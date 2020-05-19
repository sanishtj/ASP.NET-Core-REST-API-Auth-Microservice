using System.ComponentModel.DataAnnotations;

namespace AuthMicroservice.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Password and Confirm Password must match")]
        public string ConfirmNewPassword { get; set; }
    }
}
