using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AuthMicroservice.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Url]
        public string ForgotPasswordUrl { get; set; }
    }
}
