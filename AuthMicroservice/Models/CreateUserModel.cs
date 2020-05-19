using System.ComponentModel.DataAnnotations;

namespace AuthMicroservice.Models
{
    public class CreateUserModel : CreateUserBaseModel
    {
        [Required]
        public override string Password { get; set; }

    }
}
