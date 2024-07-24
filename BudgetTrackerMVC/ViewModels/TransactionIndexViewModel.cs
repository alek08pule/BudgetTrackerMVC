namespace BudgetTrackerMVC.ViewModels
{
    public class TransactionIndexViewModel
    {
        public List<TransactionVM> incomeTransactions { get; set; }
        public List<TransactionVM> expenseTransactions { get; set; }
    }
}
