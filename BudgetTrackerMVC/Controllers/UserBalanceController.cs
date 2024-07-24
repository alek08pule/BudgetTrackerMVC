using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetTrackerMVC.DataAccess;
using BudgetTrackerMVC.ViewModels.BudgetTrackerMVC.ViewModels;
using BudgetTrackerMVC.Service;

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

       
        public IActionResult Index()
        {


            var userId = _userService.GetCurrentUserIdAsync().Result;

            if (userId == null)
            {
                return Unauthorized();
            }
            var latestUserBalance = dbContext.UserBalances
                .OrderByDescending(b => b.Id)
                .FirstOrDefault();

            if (latestUserBalance == null)
            {
                return NotFound();
            }

            var userBalanceVM = new UserBalanceVM
            {
                TotalIncome = latestUserBalance.TotalIncome,
                TotalExpense = latestUserBalance.TotalExpense,
                AvailableMoney = latestUserBalance.AvailableMoney
            };

            return View(userBalanceVM);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateBalance(UserBalanceVM updatedUserBalanceVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var latestUserBalance = dbContext.UserBalances
                        .OrderByDescending(b => b.Id)
                        .FirstOrDefault();

                    if (latestUserBalance == null)
                    {
                        return NotFound();
                    }

                   
                    latestUserBalance.TotalIncome = updatedUserBalanceVM.TotalIncome;
                    latestUserBalance.TotalExpense = updatedUserBalanceVM.TotalExpense;

                    dbContext.Update(latestUserBalance);
                    dbContext.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Failed to update user balance. Please try again.");
                }
            }

            return View(updatedUserBalanceVM);
        }
    }
}
