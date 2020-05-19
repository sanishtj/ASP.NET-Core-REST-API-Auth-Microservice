using System.ComponentModel.DataAnnotations;

namespace AuthMicroservice.Models
{
    public class CreateUserBaseModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [Url]
        public string ConfirmUrl { get; set; }

        public virtual string Password { get; set; }
    }
}
