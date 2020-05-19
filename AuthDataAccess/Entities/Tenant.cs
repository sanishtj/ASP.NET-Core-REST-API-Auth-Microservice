using AuthDataAccess.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthDataAccess.Entities
{
    public class Tenant : BaseEntity
    {
        [Required]
        public Guid TenantId { get; set; }
        [Required]
        [MaxLength(100)]
        public string TenantName { get; set; }
        [Required]
        [MaxLength(500)]
        [EmailList(ErrorMessage = "Invalid Email")]
        public string TenantEmails { get; set; }
        [Required]
        [MaxLength(500)]
        public string TenantPhones { get; set; }

        public Guid? ParentTenantId { get; set; }
        public virtual ICollection<HRSIdentityUser> Users { get; set; }
    }
}
