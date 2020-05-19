using AuthDataAccess.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace AuthMicroservice.Models
{
    public class TenantCreationModel
    {
        [Required]
        [MaxLength(100)]
        public string TenantName { get; set; }

        [Required]
        [MaxLength(500)]
        [EmailList(ErrorMessage = "Invalid Email Supplied")]
        public string TenantEmails { get; set; }

        [Required]
        [MaxLength(500)]
        public string TenantPhones { get; set; }

        public Guid? ParentTenantId { get; set; }

    }
}
