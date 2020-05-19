using AuthMicroservice.Controllers;
using AuthMicroservice.Models;
using AuthMicroservice.Profiles;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.Tests
{
    public class AdministrationSpecs
    {
        AdministrationController administrationController;
        [OneTimeSetUp]
        public void SetupController()
        {

            var mockLogger = new Mock<ILogger<AdministrationController>>();
            var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                    new Mock<IRoleStore<IdentityRole>>().Object,
                    new IRoleValidator<IdentityRole>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
            var identityRoles = new List<IdentityRole>() { new IdentityRole("HRSUser"), new IdentityRole("SuperAdmin"), new IdentityRole("TenantAdmin") };
            mockRoleManager.Setup(r => r.Roles).Returns(Queryable.AsQueryable(identityRoles));
            IdentityError[] errors = new IdentityError[] { new IdentityError() };
            mockRoleManager.SetupSequence(r => r.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success).ReturnsAsync(IdentityResult.Failed(errors));
            mockRoleManager.Setup(r => r.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(identityRoles[0]);
            mockRoleManager.SetupSequence(r => r.UpdateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success).ReturnsAsync(IdentityResult.Failed(errors));
            mockRoleManager.SetupSequence(r => r.DeleteAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success).ReturnsAsync(IdentityResult.Failed(errors));

            administrationController = new AdministrationController(mockRoleManager.Object, Mapper, mockLogger.Object);

        }

        [Test]
        public void Should_Return_Roles_When_Called_GetRoles()
        {
            var roles = administrationController.GetRoles();
            Assert.That(roles.Result, Is.TypeOf<OkObjectResult>());

            if (roles.Result is OkObjectResult)
            {
                var newResult = roles.Result as OkObjectResult;
                var resultList = newResult.Value as List<RoleModel>;
                Assert.That(resultList.Count, Is.EqualTo(3));
                Assert.That(resultList[0].RoleName, Is.EqualTo("HRSUser"));
            }
        }

        [Test]
        public async Task Should_Create_Role_Successfully()
        {
            CreateRoleModel createRoleModel = new CreateRoleModel()
            {
                RoleName = "Test Role"
            };

            var result = await administrationController.AddRole(createRoleModel);

            Assert.That(result, Is.TypeOf<CreatedResult>());
            result = await administrationController.AddRole(createRoleModel);

            Assert.That(result, Is.TypeOf<ObjectResult>());
            var resultStat = result as ObjectResult;

            Assert.That(resultStat.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));


        }

        [Test]
        public async Task Should_Update_Role_Successfully()
        {
            UpdateRoleModel updateRoleModel = new UpdateRoleModel()
            {
                RoleName = "New Role"
            };

            var result = await administrationController.UpdateRole(Guid.NewGuid(), updateRoleModel);

            Assert.That(result, Is.TypeOf<NoContentResult>());
            result = await administrationController.UpdateRole(Guid.NewGuid(), updateRoleModel);

            Assert.That(result, Is.TypeOf<ObjectResult>());
            var resultStat = result as ObjectResult;

            Assert.That(resultStat.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        [Test]
        public async Task Should_Delete_Role_Successfully()
        {

            var result = await administrationController.DeleteRole(Guid.NewGuid());

            Assert.That(result, Is.TypeOf<NoContentResult>());

            result = await administrationController.DeleteRole(Guid.NewGuid());

            Assert.That(result, Is.TypeOf<ObjectResult>());
            var resultStat = result as ObjectResult;

            Assert.That(resultStat.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }


        private static IMapper _mapper;
        public static IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    // Auto Mapper Configurations
                    var mappingConfig = new MapperConfiguration(mc =>
                    {
                        mc.AddProfile(new AdminProfile());
                    });
                    IMapper mapper = mappingConfig.CreateMapper();
                    _mapper = mapper;
                }
                return _mapper;
            }
        }
    }
}
