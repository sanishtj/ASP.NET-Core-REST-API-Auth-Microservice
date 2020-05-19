using System.ComponentModel.DataAnnotations;

namespace AuthMicroservice.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
        public string ConfirmPassword { get; set; }
    }
}
