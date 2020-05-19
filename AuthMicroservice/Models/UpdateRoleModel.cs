using System.ComponentModel.DataAnnotations;

namespace AuthMicroservice.Models
{
    public class UpdateRoleModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
