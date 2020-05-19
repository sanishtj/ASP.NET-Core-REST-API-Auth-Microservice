using AuthDataAccess.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace AuthMicroservice.Models
{
    public class TenantUpdateModel
    {

        [MaxLength(100)]
        public string TenantName { get; set; }

        [MaxLength(500)]
        [EmailList(ErrorMessage = "Invalid Email Supplied")]
        public string TenantEmails { get; set; }

        [MaxLength(500)]
        public string TenantPhones { get; set; }

        public Guid? ParentTenantId { get; set; }
    }
}
