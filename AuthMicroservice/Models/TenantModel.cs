using System;

namespace AuthMicroservice.Models
{
    public class TenantModel : BaseModel
    {
        public Guid TenantId { get; set; }

        public string TenantName { get; set; }

        public string TenantEmails { get; set; }

        public string TenantPhones { get; set; }

        public Guid? ParentTenantId { get; set; }

    }
}
