using AuthDataAccess.Abstractions;
using AuthDataAccess.Entities;
using AuthMicroservice.Controllers;
using AuthMicroservice.Models;
using AuthMicroservice.Profiles;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace AuthMicroservice.Tests
{
    public class TenantSpecs
    {

        TenantsController tenantsController;
        [OneTimeSetUp]
        public void SetupController()
        {
            var mockTenantRepository = new Mock<ITenantRepository>();
            var mockLogger = new Mock<ILogger<TenantsController>>();
            mockTenantRepository.Setup(r => r.GetTenants()).Returns(GetTenants());
            mockTenantRepository.Setup(r => r.GetTenant(It.IsAny<Guid>())).Returns(GetTenant());
            mockTenantRepository.Setup(r => r.CreateTenant(It.IsAny<Tenant>())).Returns(GetTenant());
            mockTenantRepository.Setup(r => r.TenantExists(It.IsAny<Guid>())).Returns(true);
            mockTenantRepository.Setup(r => r.DeleteTenant(It.IsAny<Guid>()));
            mockTenantRepository.Setup(r => r.UpdateTenant(It.IsAny<Guid>()));

            var mockUnitofWork = new Mock<IUnitOfWork>();
            mockUnitofWork.Setup(u => u.TenantRepository).Returns(mockTenantRepository.Object);

            tenantsController = new TenantsController(mockUnitofWork.Object, Mapper, mockLogger.Object);
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            tenantsController.ObjectValidator = objectValidator.Object;
        }



        [Test]
        public void Should_Return_Ok_With_Results_Get_Tenants()
        {
            // Act
            ActionResult<List<TenantModel>> result = tenantsController.Get();

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            if (result.Result is OkObjectResult)
            {
                var newResult = result.Result as OkObjectResult;
                var resultList = newResult.Value as List<TenantModel>;
                Assert.That(resultList.Count, Is.EqualTo(2));
            }

        }

        [Test]
        public void Should_Return_Ok_With_Results_Get_A_Tenant()
        {
            // Act
            ActionResult<TenantModel> result = tenantsController.Get(new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb"));

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            if (result.Result is OkObjectResult)
            {
                var newResult = result.Result as OkObjectResult;
                var resultObj = newResult.Value as TenantModel;
                Assert.That(resultObj.TenantName, Is.EqualTo("Health Record Stack"));
            }

        }

        [Test]
        public void Should_Return_Error_With_Results_Get_A_Tenant()
        {
            // Act
            ActionResult<TenantModel> result = tenantsController.Get(new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb"));

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            if (result.Result is OkObjectResult)
            {
                var newResult = result.Result as OkObjectResult;
                var resultObj = newResult.Value as TenantModel;
                Assert.That(resultObj.TenantName, Is.EqualTo("Health Record Stack"));
            }

        }

        [Test]
        public void Should_Create_And_Return_A_Tenant()
        {
            TenantCreationModel tenantCreationModel = new TenantCreationModel()
            {
                TenantName = "Test Name" + Guid.NewGuid(),
                TenantEmails = "test@hrs.com",
                TenantPhones = "+1 7754645654"
            };

            ActionResult<TenantModel> result = tenantsController.Post(tenantCreationModel);
            Assert.That(result.Result, Is.TypeOf<CreatedAtRouteResult>());
        }


        [Test]
        public void Should_Update_With_Error_And_Success()
        {
            JsonPatchDocument<TenantUpdateModel> patch = new JsonPatchDocument<TenantUpdateModel>();
            patch.Replace(t => t.TenantEmails, "sanyjose85@gmail.com");
            patch.Replace(t => t.TenantName, "Hi there");
            patch.Replace(t => t.TenantPhones, "464645654");
            patch.Replace(t => t.ParentTenantId, Guid.NewGuid());

            JsonPatchDocument<TenantUpdateModel> patch1 = new JsonPatchDocument<TenantUpdateModel>();
            patch1.Replace(t => t.TenantEmails, "sanishtj@gmail.com");
            patch1.Replace(t => t.TenantName, "Hi there");
            patch1.Replace(t => t.TenantPhones, "464645654");
            patch1.Replace(t => t.ParentTenantId, Guid.NewGuid());

            ActionResult result1 = tenantsController.Update(new Guid("C5E35258-5BAD-44B5-B1BC-ED25A6C9E27A"), patch1);
            Assert.That(result1, Is.TypeOf<NoContentResult>());

        }


        [Test]
        public void Should_Delete_Work_Without_Error()
        {
            ActionResult result1 = tenantsController.Delete(new Guid("C5E35258-5BAD-44B5-B1BC-ED25A6C9E27A"));
            Assert.That(result1, Is.TypeOf<NoContentResult>());
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
                        mc.AddProfile(new TenantProfile());
                    });
                    IMapper mapper = mappingConfig.CreateMapper();
                    _mapper = mapper;
                }
                return _mapper;
            }
        }
        #region snippet_GetTenants


        private Tenant GetTenant()
        {
            return new Tenant()
            {

                IsDeleted = false,
                ParentTenantId = null,
                TenantEmails = "sanyjose85@gmail.com,sanishtj@gmail.com",
                TenantId = new System.Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb"),
                TenantName = "Health Record Stack",
                TenantPhones = "+1 7788708155,+1 7788708048"
            };

        }

        private List<Tenant> GetTenants()
        {
            var tenants = new List<Tenant>();
            tenants.Add(new Tenant()
            {

                IsDeleted = false,
                ParentTenantId = null,
                TenantEmails = "sanyjose85@gmail.com,sanishtj@gmail.com",
                TenantId = new System.Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb"),
                TenantName = "Health Record Stack",
                TenantPhones = "+1 7788708155,+1 7788708048"
            });
            tenants.Add(new Tenant()
            {
                IsDeleted = false,
                ParentTenantId = null,
                TenantEmails = "sanyjose90@gmail.com,sanishtj45@gmail.com",
                TenantId = new System.Guid("C5E35258-5BAD-44B5-B1BC-ED25A6C9E27A"),
                TenantName = "Dr.Senil",
                TenantPhones = "+91 45645645654"
            });
            return tenants;
        }
        #endregion
    }
}
