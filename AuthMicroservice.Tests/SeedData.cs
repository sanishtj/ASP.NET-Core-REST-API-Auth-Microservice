using AuthDataAccess.Entities;
using AuthDataAccess.SQLImplementation;
using Microsoft.AspNetCore.Identity;
using System;

namespace AuthMicroservice.Tests
{
    public static class SeedData
    {
        public static void PopulateTestData(AuthManagementDbContext dbContext)
        {
            dbContext.Tenants.Add(new AuthDataAccess.Entities.Tenant()
            {
                CreatedOn = Convert.ToDateTime("2020-05-05 11:36:22.7600000"),
                CreatedUserId = new Guid("88A36552-A48E-4EC6-AFD0-AB8201918EED"),
                IsDeleted = false,
                ModifiedOn = null,
                ModifiedUserId = null,
                ParentTenantId = null,
                TenantEmails = "sanyjose85@gmail.com,sanishtj@gmail.com",
                TenantId = new Guid("187E5BBF-9F5D-45BD-B8C4-0B894E71DC1F"),
                TenantName = "Health Record Stack",
                TenantPhones = "+1 7788708155,+1 7788708048"
            });
            dbContext.Tenants.Add(new AuthDataAccess.Entities.Tenant()
            {
                CreatedOn = Convert.ToDateTime("2020-05-05 11:36:22.7600000"),
                CreatedUserId = new Guid("88A36552-A48E-4EC6-AFD0-AB8201918EED"),
                IsDeleted = false,
                ModifiedOn = null,
                ModifiedUserId = null,
                ParentTenantId = null,
                TenantEmails = "sanyjose90@gmail.com,sanishtj45@gmail.com",
                TenantId = new Guid("A7BC6C3B-74DE-4F5B-B9C9-94545ABCC6C3"),
                TenantName = "Dr.Senil",
                TenantPhones = "+91 45645645654"
            });
            dbContext.Tenants.Add(new AuthDataAccess.Entities.Tenant()
            {
                CreatedOn = Convert.ToDateTime("2020-05-05 11:36:22.7600000"),
                CreatedUserId = new Guid("88A36552-A48E-4EC6-AFD0-AB8201918EED"),
                IsDeleted = false,
                ModifiedOn = null,
                ModifiedUserId = null,
                ParentTenantId = null,
                TenantEmails = "sanyjose90@gmail.com,sanishtj45@gmail.com",
                TenantId = new Guid("1183DB51-77B8-4B98-84EE-9FD4C6F2ADFB"),
                TenantName = "Dr.Sanish",
                TenantPhones = "+91 45645645654"
            });

            HRSIdentityUser user = new HRSIdentityUser()
            {
                Email = "sanyjose85@gmail.com",
                EmailConfirmed = true,
                NormalizedEmail = "SANYJOSE85@GMAIL.COM",
                Id = new Guid("359036a1-4a00-4519-946e-4534c01c6246").ToString(),
                PasswordHash = "AQAAAAEAACcQAAAAEEbeR5Lh0lutGhy3HI/Nn8TrgMRRyUPWc7R2ggXIJv/BpGP6NY4uNjmchQDrDdqCGw==",
                PhoneNumber = "+91 45645645654",
                TenantId = new Guid("1183DB51-77B8-4B98-84EE-9FD4C6F2ADFB"),
                UserName = "sanyjose85@gmail.com",
                NormalizedUserName = "SANYJOSE85@GMAIL.COM"
            };

            dbContext.Users.Add(user);

            user = new HRSIdentityUser()
            {
                Email = "sanyjose90@gmail.com",
                EmailConfirmed = true,
                NormalizedEmail = "SANYJOSE90@GMAIL.COM",
                Id = new Guid("e3d3ab60-a3b5-4678-963e-e3103e71f1a5").ToString(),
                PasswordHash = "AQAAAAEAACcQAAAAEDj1a3BsKXLiqoDrM09Hfim+oG12O1lnzdpSn4JjWLq0QY7ga4Dacn5AfV79I3HihQ==",
                PhoneNumber = "+91 45645645654",
                TenantId = new Guid("1183DB51-77B8-4B98-84EE-9FD4C6F2ADFB"),
                UserName = "sanyjose90@gmail.com",
                NormalizedUserName = "SANYJOSE90@GMAIL.COM"
            };

            dbContext.Users.Add(user);

            IdentityRole role = new IdentityRole()
            {
                Id = new Guid("359036a1-4a00-4519-946e-4534c01c6246").ToString(),
                Name = "HRSUser",
                NormalizedName = "HRSUSER"
            };
            dbContext.Roles.Add(role);
            role = new IdentityRole()
            {
                Id = new Guid("ab4cd6d9-43c0-4353-a218-0b68b1a414b4").ToString(),
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN"
            };
            dbContext.Roles.Add(role);

            role = new IdentityRole()
            {
                Id = new Guid("e70b15de-fd8a-4423-9f56-566c74930dd0").ToString(),
                Name = "TestAdmin",
                NormalizedName = "TESTADMIN"

            };
            dbContext.Roles.Add(role);

            role = new IdentityRole()
            {
                Id = new Guid("dd3f46ee-3e4b-47d4-8da9-c1ad4aae8afd").ToString(),
                Name = "TestAdminForDelete",
                NormalizedName = "TESTADMINFORDELETE"
            };
            dbContext.Roles.Add(role);

            IdentityUserRole<string> userRole = new IdentityUserRole<string>()
            {
                RoleId = new Guid("ab4cd6d9-43c0-4353-a218-0b68b1a414b4").ToString(),
                UserId = new Guid("359036a1-4a00-4519-946e-4534c01c6246").ToString()
            };
            dbContext.UserRoles.Add(userRole);

            userRole = new IdentityUserRole<string>()
            {
                RoleId = new Guid("ab4cd6d9-43c0-4353-a218-0b68b1a414b4").ToString(),
                UserId = new Guid("e3d3ab60-a3b5-4678-963e-e3103e71f1a5").ToString()
            };

            dbContext.UserRoles.Add(userRole);

            dbContext.SaveChanges();
        }


    }
}
