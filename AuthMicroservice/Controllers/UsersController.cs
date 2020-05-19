using AuthDataAccess.Entities;
using AuthMicroservice.Models;
using AuthMicroservice.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AuthMicroservice.Controllers
{
    [Route("api/tenants/{tenantid}/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {

        UserManager<HRSIdentityUser> _userManager;
        SignInManager<HRSIdentityUser> _signInManager;
        IMapper _mapper;
        IEmailSender _emailSender;
        ILogger<UsersController> _log;
        IConfiguration Configuration { get; }

        public UsersController(UserManager<HRSIdentityUser> userManager, IMapper mapper, ILogger<UsersController> log, IEmailSender emailSender, SignInManager<HRSIdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _log = log;
            Configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromRoute]Guid tenantid, [FromBody] CreateUserModel user)
        {
            return await CommonRegister(tenantid, user, new string[] { "HRSUser" });
        }

        [HttpPost]
        [Route("tregister")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Register([FromRoute]Guid tenantid, [FromBody]CreateTUserModel user)
        {
            return await CommonRegister(tenantid, user, user.Roles);
        }

        private async Task<ActionResult> CommonRegister<T>(Guid tenantid, T user, string[] roles) where T : CreateUserBaseModel
        {
            HRSIdentityUser hRSIdentityUser = _mapper.Map<HRSIdentityUser>(user);
            hRSIdentityUser.TenantId = tenantid;
            IdentityResult result;
            if (string.IsNullOrEmpty(user.Password))
            {
                result = await _userManager.CreateAsync(hRSIdentityUser);
            }
            else
            {
                result = await _userManager.CreateAsync(hRSIdentityUser, user.Password);
            }


            if (result.Succeeded)
            {
                try
                {
                    var roleResult = await _userManager.AddToRolesAsync(hRSIdentityUser, roles);
                    if (roleResult.Succeeded)
                    {
                        // Create token
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(hRSIdentityUser);

                        var callbackUrl = user.ConfirmUrl + "?token=" + HttpUtility.UrlEncode(token) + "&userid=" + hRSIdentityUser.Id;

                        // Send email for Confirmation
                        await _emailSender.SendEmailAsync(hRSIdentityUser.Email, "Confirm Email", "Call back URL -> " + callbackUrl + "");
                        return Ok("User Created");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            foreach (var error in result.Errors)
            {
                _log.LogError(error.Description, error);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
        }

        //public async Task<IActionResult> ResendEmailConfirmation()
        //{

        //}

        [HttpGet]
        [Route("confirmemail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token, string userid)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userid))
            {
                return BadRequest("Invalid URL");
            }

            var user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok("Email Confirmed");
            }


            foreach (var error in result.Errors)
            {
                _log.LogError(error.Description, error);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
        }

        [HttpPost]
        [Route("setpassword", Name = "setpassword")]
        [AllowAnonymous]
        public async Task<ActionResult> SetPassword(Guid tenantid, SetPasswordModel setPasswordModel)
        {
            // Check if email exists
            var user = await _userManager.FindByEmailAsync(setPasswordModel.Email);
            if (user == null)
            {
                return NotFound("User not found");
            }
            // Check if email confirmed
            if (user != null && !user.EmailConfirmed)
            {
                return BadRequest("Email not confirmed yet");
            }

            if (user != null && user.PasswordHash != null)
            {
                return BadRequest("Password already set. Use forgot password");
            }
            // Add password 
            var result = await _userManager.AddPasswordAsync(user, setPasswordModel.Password);

            if (result.Succeeded)
            {
                return NoContent();
            }

            foreach (var error in result.Errors)
            {
                _log.LogError(error.Description, error);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
        }


        [HttpPost]
        [Route("login", Name = "Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(Guid tenantid, AuthenticateUserModel authUser)
        {
            var user = await _userManager.FindByEmailAsync(authUser.Email);

            if (user == null)
            {
                return NotFound("User not found");
            }

            if (user != null && !user.EmailConfirmed && await _userManager.CheckPasswordAsync(user, authUser.Password))
            {
                return BadRequest("Email not confirmed yet");
            }

            var result = await _signInManager.PasswordSignInAsync(authUser.Email, authUser.Password, false, false);

            if (result.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                // To-Do: HRS parent site to be allowed for all users. Add HRSUser role to any tenant // && !roles.Contains("HRSUser")
                if (tenantid != user.TenantId)
                {
                    return BadRequest("Your account belongs to another service. Please contact Health Record Stack Admin");
                }

                var claims = new List<Claim> {
                    new Claim(JwtRegisteredClaimNames.Iss,"Health Record Stack Auth"),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Aud, user.Id),
                    new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddMinutes(20)).ToUnixTimeSeconds().ToString()),
                    new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Tenant", user.TenantId.ToString()),
                    };

                foreach (var userRole in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                    //var role = await _roleManager.FindByNameAsync(userRole);
                    //if (role != null)
                    //{
                    //    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    //    foreach (Claim roleClaim in roleClaims)
                    //    {
                    //        claims.Add(roleClaim);
                    //    }
                    //}
                }

                var secretBytes = Encoding.UTF8.GetBytes(Configuration["HealthRecordStackSecret"]);
                var key = new SymmetricSecurityKey(secretBytes);
                var algorithm = SecurityAlgorithms.HmacSha256;
                var signingCredentials = new SigningCredentials(key, algorithm);

                var token = new JwtSecurityToken(null, null, claims, null, null, signingCredentials);

                var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new TokenModel { Token = tokenJson });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Couldn't login");


        }

        [HttpPost]
        [Route("forgotpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);

            if (user == null)
            {
                return NotFound("User not found");
            }

            if (user != null && !user.EmailConfirmed)
            {
                return BadRequest("Email not confirmed yet");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = forgotPasswordModel.ForgotPasswordUrl + "?token=" + HttpUtility.UrlEncode(token) + "&email=" + forgotPasswordModel.Email;
            await _emailSender.SendEmailAsync(forgotPasswordModel.Email, "Reset Password", "Call back URL -> " + callbackUrl);

            return Ok("Reset Link Send to mail");
        }

        [HttpPost]
        [Route("resetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string token, string email, ResetPasswordModel resetPasswordModel)
        {

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordModel.Password);
            if (result.Succeeded)
            {
                return Ok("Password reset success");
            }

            foreach (var error in result.Errors)
            {
                _log.LogError(error.Description, error);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
        }

        [HttpPost]
        [Route("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {

            var user = await _userManager.FindByEmailAsync(changePasswordModel.Email);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordModel.OldPassword, changePasswordModel.NewPassword);
            if (result.Succeeded)
            {
                return Ok("Password change success");
            }

            foreach (var error in result.Errors)
            {
                _log.LogError(error.Description, error);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);

        }
    }
}
