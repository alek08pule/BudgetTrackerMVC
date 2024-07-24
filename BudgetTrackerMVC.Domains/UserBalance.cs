using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTrackerMVC.Domains
{
    public class UserBalance
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; } = null!;
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalExpense { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal AvailableMoney => TotalIncome - TotalExpense;


        public User User { get; set; } = null!;
    }
}
