using BudgetTrackerMVC.Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTrackerMVC.Domains
{
    public class Transaction
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = null!;
        [Required]
        [Precision(16,2)]
        public int Amount { get; set; }
        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public User User { get; set; } = null!;
    }
}