// FILE: Models/LoginViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace ContractMonthlyClaim.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username or Email")]
        public string UserNameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}