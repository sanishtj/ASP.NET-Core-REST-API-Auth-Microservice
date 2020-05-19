using AuthDataAccess.Entities;
using System;
using System.Collections.Generic;

namespace AuthDataAccess.Abstractions
{
    public interface ITenantRepository
    {
        List<Tenant> GetTenants();
        Tenant GetTenant(Guid tenantId);
        Tenant CreateTenant(Tenant tenant);
        bool TenantExists(Guid tenantId);
        void DeleteTenant(Guid tenantId);
        void UpdateTenant(Guid tenantId);
    }
}
