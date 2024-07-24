
using System.ComponentModel.DataAnnotations;
namespace BudgetTrackerMVC.ViewModels
{
    
    namespace BudgetTrackerMVC.ViewModels
    {
        public class UserBalanceVM
        {
            public decimal TotalIncome { get; set; }
            public decimal TotalExpense { get; set; }
            public decimal AvailableMoney { get; set; }
        }
    }

}
