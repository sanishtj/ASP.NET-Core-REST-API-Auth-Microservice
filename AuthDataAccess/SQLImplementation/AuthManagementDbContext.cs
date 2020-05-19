using AuthDataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthDataAccess.SQLImplementation
{
    public class AuthManagementDbContext : IdentityDbContext<HRSIdentityUser>
    {
        public AuthManagementDbContext(DbContextOptions<AuthManagementDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<HRSIdentityUser>().Property(u => u.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Tenant>().Property(t => t.TenantId).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Tenant>().Property(t => t.CreatedOn).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Tenant>().Property(t => t.ModifiedOn).HasDefaultValueSql("GETDATE()").IsRequired(false);
            modelBuilder.Entity<Tenant>().HasIndex(t => t.TenantName).IsUnique();

        }
    }
}
