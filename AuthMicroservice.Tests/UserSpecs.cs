using AuthDataAccess.Entities;
using AuthMicroservice.Controllers;
using AuthMicroservice.Models;
using AuthMicroservice.Profiles;
using AuthMicroservice.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthMicroservice.Tests
{

    public class UserSpecs
    {
        UsersController usersController;
        [OneTimeSetUp]
        public void SetupController()
        {

            var mockLogger = new Mock<ILogger<UsersController>>();
            var mockUserManager = new Mock<UserManager<HRSIdentityUser>>(
                    new Mock<IUserStore<HRSIdentityUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<HRSIdentityUser>>().Object,
                    new IUserValidator<HRSIdentityUser>[0],
                    new IPasswordValidator<HRSIdentityUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<HRSIdentityUser>>>().Object);

            var mockSignInManager = GetMockSignInManager();

            var mockEmail = new Mock<IEmailSender>();
            IdentityError[] errors = new IdentityError[] { new IdentityError() };
            mockUserManager.SetupSequence(r => r.CreateAsync(It.IsAny<HRSIdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).ReturnsAsync(IdentityResult.Failed(errors)).ReturnsAsync(IdentityResult.Success).ReturnsAsync(IdentityResult.Failed(errors));
            mockUserManager.SetupSequence(r => r.CreateAsync(It.IsAny<HRSIdentityUser>())).ReturnsAsync(IdentityResult.Success).ReturnsAsync(IdentityResult.Failed(errors)).ReturnsAsync(IdentityResult.Success).ReturnsAsync(IdentityResult.Failed(errors));
            mockUserManager.Setup(r => r.AddToRolesAsync(It.IsAny<HRSIdentityUser>(), It.IsAny<string[]>())).ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(r => r.GenerateEmailConfirmationTokenAsync(It.IsAny<HRSIdentityUser>())).ReturnsAsync("Token");
            mockUserManager.SetupSequence(r => r.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new HRSIdentityUser() { }).ReturnsAsync((HRSIdentityUser)null).ReturnsAsync(new HRSIdentityUser() { });
            mockUserManager.SetupSequence(r => r.ConfirmEmailAsync(It.IsAny<HRSIdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).ReturnsAsync(IdentityResult.Failed(errors));

            mockUserManager.SetupSequence(r => r.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((HRSIdentityUser)null)
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = false, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = true, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = true, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = true, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync((HRSIdentityUser)null)
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = false, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = true, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = true, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = true, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync((HRSIdentityUser)null)
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = false, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = true, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = true, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = true, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync((HRSIdentityUser)null)
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = false, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = true, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = true, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = true, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync((HRSIdentityUser)null)
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = false, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = true, PasswordHash = "somepassword", TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = true, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = true, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") })
                .ReturnsAsync(new HRSIdentityUser() { EmailConfirmed = true, TenantId = new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb") });

            mockUserManager.SetupSequence(r => r.CheckPasswordAsync(It.IsAny<HRSIdentityUser>(), It.IsAny<string>())).ReturnsAsync(true);
            mockSignInManager.SetupSequence(r => r.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success)
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success)
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed)
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            mockUserManager.SetupSequence(r => r.GetRolesAsync(It.IsAny<HRSIdentityUser>())).ReturnsAsync(new List<string>() { "TenantAdmin", "HRSUser" })
                .ReturnsAsync(new List<string>() { "TenantAdmin", "HRSUser" })
                .ReturnsAsync(new List<string>() { "TenantAdmin", "HRSUser" });

            mockUserManager.Setup(r => r.GeneratePasswordResetTokenAsync(It.IsAny<HRSIdentityUser>())).ReturnsAsync("Token");
            mockUserManager.SetupSequence(r => r.ResetPasswordAsync(It.IsAny<HRSIdentityUser>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success)
                .ReturnsAsync(IdentityResult.Failed(errors)).ReturnsAsync(IdentityResult.Success).ReturnsAsync(IdentityResult.Success);

            mockUserManager.SetupSequence(r => r.ChangePasswordAsync(It.IsAny<HRSIdentityUser>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success)
                .ReturnsAsync(IdentityResult.Failed(errors)).ReturnsAsync(IdentityResult.Success).ReturnsAsync(IdentityResult.Success);

            mockUserManager.SetupSequence(r => r.AddPasswordAsync(It.IsAny<HRSIdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success)
               .ReturnsAsync(IdentityResult.Failed(errors)).ReturnsAsync(IdentityResult.Success).ReturnsAsync(IdentityResult.Success);

            mockEmail.Setup(r => r.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            //var mockOptions = new Mock<IEmailSender>();
            //var mockOptions = new Mock<IOptions<AuthMessageSenderOptions>>();
            var option = Options.Create(new AuthMessageSenderOptions()
            {
                HealthRecordStackSecret = "sometstxvxxdgdfgfgfgdfgdfgdfgdf",
                SendGridKey = "sdfsdfsdf",
                SendGridUser = "sdfsdfsdfs"
            });
            usersController = new UsersController(mockUserManager.Object, Mapper, mockLogger.Object, mockEmail.Object, mockSignInManager.Object, option);

        }

        [Test]
        public async Task Should_HRSUser_Register_User()
        {
            CreateUserModel user = new CreateUserModel()
            {
                Email = "test@test.com",
                Password = "1qaz!QAZ1",
                ConfirmUrl = "https://www.test.com/ConfirmEmail"
            };

            var result = await usersController.Register(Guid.NewGuid(), user);

            Assert.That(result, Is.TypeOf<OkObjectResult>());

            result = await usersController.Register(Guid.NewGuid(), user);

            Assert.That(result, Is.TypeOf<ObjectResult>());
            var resultStat = result as ObjectResult;

            Assert.That(resultStat.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }


        [Test]
        public async Task Should_Tenant_User_Register_User()
        {
            CreateTUserModel user = new CreateTUserModel()
            {
                Email = "test" + Guid.NewGuid() + "@test.com",
                ConfirmUrl = "https://www.test.com/ConfirmEmail",
                Roles = new string[] { "HRSUser" }
            };

            var result = await usersController.Register(Guid.NewGuid(), user);

            Assert.That(result, Is.TypeOf<OkObjectResult>());

            result = await usersController.Register(Guid.NewGuid(), user);

            Assert.That(result, Is.TypeOf<ObjectResult>());
            var resultStat = result as ObjectResult;

            Assert.That(resultStat.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        [Test]
        public async Task Should_Confirmation_Work()
        {
            var result = await usersController.ConfirmEmail("", "");

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            result = await usersController.ConfirmEmail("token", "userid");

            Assert.That(result, Is.TypeOf<OkObjectResult>());

            result = await usersController.ConfirmEmail("token", "userid");

            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());

            result = await usersController.ConfirmEmail("token", "userid");

            Assert.That(result, Is.TypeOf<ObjectResult>());
            var resultStat = result as ObjectResult;

            Assert.That(resultStat.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }


        [Test]
        public async Task Should_Login_Work()
        {
            AuthenticateUserModel authenticateUserModel = new AuthenticateUserModel()
            {
                Email = "test@test.com",
                Password = "1qaz!QAZ1"
            };

            var result = await usersController.Login(Guid.NewGuid(), authenticateUserModel);
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());

            result = await usersController.Login(Guid.NewGuid(), authenticateUserModel);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            Assert.That((result as BadRequestObjectResult).Value, Is.EqualTo("Email not confirmed yet"));

            result = await usersController.Login(Guid.NewGuid(), authenticateUserModel);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            Assert.That((result as BadRequestObjectResult).Value, Is.EqualTo("Your account belongs to another service. Please contact Health Record Stack Admin"));

            result = await usersController.Login(new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb"), authenticateUserModel);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            result = await usersController.Login(new Guid("1183db51-77b8-4b98-84ee-9fd4c6f2adfb"), authenticateUserModel);
            Assert.That(result, Is.TypeOf<ObjectResult>());
            Assert.That((result as ObjectResult).StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }


        [Test]
        public async Task Should_ForgotPassword_Work()
        {
            ForgotPasswordModel forgotPasswordModel = new ForgotPasswordModel() { Email = "test@test.com", ForgotPasswordUrl = "https://test.com/forgotpassword" };

            var result = await usersController.ForgotPassword(forgotPasswordModel);
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());

            result = await usersController.ForgotPassword(forgotPasswordModel);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            Assert.That((result as BadRequestObjectResult).Value, Is.EqualTo("Email not confirmed yet"));

            result = await usersController.ForgotPassword(forgotPasswordModel);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            result = await usersController.ForgotPassword(forgotPasswordModel);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            result = await usersController.ForgotPassword(forgotPasswordModel);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

        }


        [Test]
        public async Task Should_ResetPassword_Work()
        {
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel()
            {
                Password = "testpassword",
                ConfirmPassword = "testpassword"
            };

            var result = await usersController.ResetPassword("token", "test@sdf.com", resetPasswordModel);
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
            result = await usersController.ResetPassword("token", "test@sdf.com", resetPasswordModel);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            result = await usersController.ResetPassword("token", "test@sdf.com", resetPasswordModel);
            Assert.That(result, Is.TypeOf<ObjectResult>());
            Assert.That((result as ObjectResult).StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));

            result = await usersController.ResetPassword("token", "test@sdf.com", resetPasswordModel);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            result = await usersController.ResetPassword("token", "test@sdf.com", resetPasswordModel);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task Should_ChangePassword_Work()
        {
            ChangePasswordModel changePasswordModel = new ChangePasswordModel()
            {
                Email = "test@test.com",
                OldPassword = "testpassword",
                NewPassword = "newpassword",
                ConfirmNewPassword = "newpassword"
            };

            var result = await usersController.ChangePassword(changePasswordModel);
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());

            result = await usersController.ChangePassword(changePasswordModel);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            result = await usersController.ChangePassword(changePasswordModel);
            Assert.That(result, Is.TypeOf<ObjectResult>());
            Assert.That((result as ObjectResult).StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));

            result = await usersController.ChangePassword(changePasswordModel);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            result = await usersController.ChangePassword(changePasswordModel);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task Should_SetPassword_Work()
        {
            SetPasswordModel setPassword = new SetPasswordModel()
            {
                Email = "test@test.com",
                Password = "testpassword",
                ConfirmPassword = "testpassword"
            };

            var result = await usersController.SetPassword(Guid.NewGuid(), setPassword);
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());

            result = await usersController.SetPassword(Guid.NewGuid(), setPassword);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            result = await usersController.SetPassword(Guid.NewGuid(), setPassword);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            result = await usersController.SetPassword(Guid.NewGuid(), setPassword);
            Assert.That(result, Is.TypeOf<NoContentResult>());

            result = await usersController.SetPassword(Guid.NewGuid(), setPassword);
            Assert.That(result, Is.TypeOf<ObjectResult>());
            Assert.That((result as ObjectResult).StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));

        }



        private Mock<SignInManager<HRSIdentityUser>> GetMockSignInManager()
        {
            var mockUserManager = new Mock<UserManager<HRSIdentityUser>>(
                    new Mock<IUserStore<HRSIdentityUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<HRSIdentityUser>>().Object,
                    new IUserValidator<HRSIdentityUser>[0],
                    new IPasswordValidator<HRSIdentityUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<HRSIdentityUser>>>().Object);
            var ctxAccessor = new HttpContextAccessor();
            var mockClaimsPrinFact = new Mock<IUserClaimsPrincipalFactory<HRSIdentityUser>>();
            var mockOpts = new Mock<IOptions<IdentityOptions>>();
            var mockLogger = new Mock<ILogger<SignInManager<HRSIdentityUser>>>();
            var mockIAuthenticationSchemeProvider = new Mock<IAuthenticationSchemeProvider>();
            var mockIUserConfirmation = new Mock<IUserConfirmation<HRSIdentityUser>>();
            return new Mock<SignInManager<HRSIdentityUser>>(mockUserManager.Object, ctxAccessor, mockClaimsPrinFact.Object, mockOpts.Object, mockLogger.Object, mockIAuthenticationSchemeProvider.Object, mockIUserConfirmation.Object);
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
