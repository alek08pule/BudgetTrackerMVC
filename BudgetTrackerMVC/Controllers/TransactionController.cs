using BudgetTrackerMVC.DataAccess;
using BudgetTrackerMVC.Domains;
using BudgetTrackerMVC.Service;
using BudgetTrackerMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;

namespace BudgetTrackerMVC.Controllers
{
    public class TransactionController : Controller
    {

        private readonly BudgetTrackerDbContext dbContext;
        private readonly IUserService _userService;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(BudgetTrackerDbContext dbContext, IUserService userService, ILogger<TransactionController> logger)
        {
            this.dbContext = dbContext;
            _userService = userService;
            _logger = logger;
        }

        public async Task<IActionResult> GetTransactions()
        {
            var userId = await _userService.GetCurrentUserIdAsync();

            if (userId == null)
            {
                return Unauthorized();
            }

            var transactions = await dbContext.Transactions
                    .Where(t => t.UserId == userId)
                     .Select(t => new TransactionVM
                     {
                         Id = t.Id,
                         Description = t.Description,
                         Amount = t.Amount,
                         Category = t.Category
                     })
                    .ToListAsync();

            _logger.LogInformation($"Retrieved {transactions.Count} transactions for user {userId}");

            var viewModel = new TransactionIndexViewModel
            {
                incomeTransactions = transactions.Where(t => t.Category == "income").ToList(),
                expenseTransactions = transactions.Where(t => t.Category == "expense").ToList()
            };

            return View(viewModel);

        }

        [HttpPost]
        public IActionResult AddTransaction(TransactionVM transactionVM)
        {
            if (!ModelState.IsValid)
            {
                return View(transactionVM);
            }

            var userId = _userService.GetCurrentUserIdAsync().Result;

            if (userId == null)
            {
                return Unauthorized();
            }
            Transaction transaction = new Transaction()
            {
                UserId = userId,
                Description = transactionVM.Description,
                Category = transactionVM.Category,
                Amount = transactionVM.Amount,
                CreatedAt = DateTime.Now,
            };
            dbContext.Transactions.Add(transaction);
            dbContext.SaveChanges();

            UpdateUserBalance(userId, transaction);
            var userBalance = dbContext.UserBalances.SingleOrDefault(ub => ub.UserId == userId);

            return Json(new
            {
                totalIncome = userBalance.TotalIncome,
                totalExpense = userBalance.TotalExpense,
                availableMoney = userBalance.AvailableMoney
            });


        }
        private void UpdateUserBalance(string userId, Transaction transaction)
        {
            var userBalance = dbContext.UserBalances.SingleOrDefault(ub => ub.UserId == userId);

            if (userBalance == null)
            {
                userBalance = new UserBalance
                {
                    UserId = userId,
                    TotalIncome = 0,
                    TotalExpense = 0
                };

                dbContext.UserBalances.Add(userBalance);
            }

            if (transaction.Category == "income")
            {
                userBalance.TotalIncome += transaction.Amount;
            }
            else if (transaction.Category == "expense")
            {
                userBalance.TotalExpense += transaction.Amount;
            }

            dbContext.SaveChanges();
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteTransaction([FromBody] JsonElement data)
        {
            if (data.TryGetProperty("id", out JsonElement idElement) && idElement.TryGetInt32(out int id))
            {
                if (id <= 0)
                {
                    return BadRequest(new { success = false, message = "Invalid ID" });
                }

                var transaction = await dbContext.Transactions.SingleOrDefaultAsync(t => t.Id == id);

                if (transaction == null)
                {
                    return NotFound(new { success = false, message = "Transaction not found" });
                }

                var userId = transaction.UserId;

                dbContext.Transactions.Remove(transaction);
                await dbContext.SaveChangesAsync();

                UpdateUserBalanceAfterDeletion(userId, transaction);
                var userBalance = await dbContext.UserBalances.SingleOrDefaultAsync(ub => ub.UserId == userId);

                if (userBalance == null)
                {
                    return NotFound(new { success = false, message = "User balance not found" });
                }

                return Json(new
                {
                    success = true,
                    totalIncome = userBalance.TotalIncome,
                    totalExpense = userBalance.TotalExpense,
                    availableMoney = userBalance.AvailableMoney
                });
            }

            return BadRequest(new { success = false, message = "Invalid data" });
        }

        private void UpdateUserBalanceAfterDeletion(string userId, Transaction transaction)
        {
            var userBalance = dbContext.UserBalances.SingleOrDefault(ub => ub.UserId == userId);

            if (userBalance != null)
            {
                if (transaction.Category == "income")
                {
                    userBalance.TotalIncome -= transaction.Amount;
                }
                else if (transaction.Category == "expense")
                {
                    userBalance.TotalExpense -= transaction.Amount;
                }

                dbContext.SaveChanges();
            }
        }
    }
}


