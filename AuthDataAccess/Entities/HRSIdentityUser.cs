using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace AuthDataAccess.Entities
{
    public class HRSIdentityUser : IdentityUser
    {
        [Required]
        public Guid TenantId { get; set; }

        public Tenant Tenant { get; set; }

    }
}
