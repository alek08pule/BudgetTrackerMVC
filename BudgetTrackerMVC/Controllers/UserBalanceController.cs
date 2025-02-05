using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetTrackerMVC.DataAccess;
using BudgetTrackerMVC.ViewModels;
using BudgetTrackerMVC.Service;
using BudgetTrackerMVC.Domains;
using BudgetTrackerMVC.ViewModels.BudgetTrackerMVC.ViewModels;

namespace BudgetTrackerMVC.Controllers
{
    public class UserBalanceController : Controller
    {
        private readonly BudgetTrackerDbContext dbContext;
        private readonly IUserService _userService;

        public UserBalanceController(BudgetTrackerDbContext dbContext, IUserService userService)
        {
            this.dbContext = dbContext;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = await _userService.GetCurrentUserIdAsync();
            if (userId == null)
            {
                return Unauthorized();
            }

            var latestUserBalance = await dbContext.UserBalances
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.Id)
                .FirstOrDefaultAsync();

            if (latestUserBalance == null)
            {
                // Initialize a new balance if none exists
                latestUserBalance = new UserBalance
                {
                    UserId = userId,
                    TotalIncome = 0,
                    TotalExpense = 0,
                };

                dbContext.UserBalances.Add(latestUserBalance);
                await dbContext.SaveChangesAsync();
            }

            var userBalanceVM = new UserBalanceVM
            {
                TotalIncome = latestUserBalance.TotalIncome,
                TotalExpense = latestUserBalance.TotalExpense,
                AvailableMoney = latestUserBalance.AvailableMoney
            };

            return View(userBalanceVM);
        }
        public async Task<IActionResult> GetChartData()
        {
            var userId = await _userService.GetCurrentUserIdAsync();
            var latestUserBalance = await dbContext.UserBalances
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.Id)
                .FirstOrDefaultAsync();

            var chartData = latestUserBalance != null
                ? new List<decimal> { latestUserBalance.TotalIncome, latestUserBalance.TotalExpense, latestUserBalance.AvailableMoney }
                : new List<decimal> { 0, 0, 0 };

            return View(chartData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBalance(UserBalanceVM updatedUserBalanceVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = await _userService.GetCurrentUserIdAsync();
                    if (userId == null)
                    {
                        return Unauthorized();
                    }

                    var latestUserBalance = await dbContext.UserBalances
                        .Where(b => b.UserId == userId)
                        .OrderByDescending(b => b.Id)
                        .FirstOrDefaultAsync();

                    if (latestUserBalance == null)
                    {
                        return NotFound();
                    }

                    // Update the balance
                    latestUserBalance.TotalIncome = updatedUserBalanceVM.TotalIncome;
                    latestUserBalance.TotalExpense = updatedUserBalanceVM.TotalExpense;

                    dbContext.Update(latestUserBalance);
                    await dbContext.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Failed to update user balance: {ex.Message}");
                }
            }

            return View(updatedUserBalanceVM);
        }
    }
}
