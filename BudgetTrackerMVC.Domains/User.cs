using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BudgetTrackerMVC.Domains
{
    public class User : IdentityUser
    {
     
        [MaxLength(100)]
        [Required]
        public string? Firstname { get; set; }
        [MaxLength(100)]
        [Required]
        public string? Lastname { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
        public UserBalance? UserBalances{ get; set;}

    }
}
