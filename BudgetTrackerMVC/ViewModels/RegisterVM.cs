using System.ComponentModel.DataAnnotations;

namespace BudgetTrackerMVC.ViewModels

{
    public class RegisterVM
    {
        [Required]
        public string? Firstname { get; set; }
        [Required]
        public string? Lastname { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password dont't match")]
        public string? ConfirmPassword { get; set; }

    }
}
