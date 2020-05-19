using System.ComponentModel.DataAnnotations;

namespace AuthMicroservice.Models
{
    public class CreateRoleModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
