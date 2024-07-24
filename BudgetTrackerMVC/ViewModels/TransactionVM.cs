
using System.ComponentModel.DataAnnotations;

namespace BudgetTrackerMVC.ViewModels
{
    public class TransactionVM
    {
        [Required]
        [MaxLength(100)]
        public string? Description { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Category { get; set; }
        public int Id { get; set; }
    }
}
