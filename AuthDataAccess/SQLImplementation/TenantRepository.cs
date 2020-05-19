using AuthDataAccess.Abstractions;
using AuthDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthDataAccess.SQLImplementation
{
    public class TenantRepository : ITenantRepository
    {
        private AuthManagementDbContext _context;
        public TenantRepository(AuthManagementDbContext context)
        {
            _context = context;
        }

        public Tenant CreateTenant(Tenant tenant)
        {
            var addedTenant = _context.Tenants.Add(tenant);
            return addedTenant.Entity;
        }

        public void DeleteTenant(Guid tenantId)
        {
            Tenant tenant = GetTenant(tenantId);
            tenant.IsDeleted = true;
            _context.Tenants.Update(tenant);
        }

        public Tenant GetTenant(Guid tenantId)
        {
            return _context.Tenants.FirstOrDefault(t => t.TenantId == tenantId);
        }

        public List<Tenant> GetTenants()
        {
            return _context.Tenants.ToList();
        }

        public bool TenantExists(Guid tenantId)
        {
            Tenant tenant = GetTenant(tenantId);

            return tenant != null;
        }

        public void UpdateTenant(Guid tenantId)
        {

        }
    }
}
