using AuthMicroservice.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.Controllers
{
    [Route("api/admin/role")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class AdministrationController : ControllerBase
    {
        RoleManager<IdentityRole> _roleManager;
        IMapper _mapper;
        ILogger<AdministrationController> _log;
        public AdministrationController(RoleManager<IdentityRole> roleManager, IMapper mapper, ILogger<AdministrationController> log)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _log = log;
        }

        [HttpGet]
        public ActionResult<List<RoleModel>> GetRoles()
        {
            List<IdentityRole> roles = _roleManager.Roles.ToList();

            List<RoleModel> rolesModel = _mapper.Map<List<RoleModel>>(roles);

            return Ok(rolesModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(CreateRoleModel role)
        {
            IdentityRole identityRole = _mapper.Map<IdentityRole>(role);
            var result = await _roleManager.CreateAsync(identityRole);

            if (result.Succeeded)
            {
                return Created("", identityRole);
            }

            foreach (var error in result.Errors)
            {
                _log.LogError(error.Description, error);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
        }

        [HttpPut]
        [Route("{roleId}")]
        public async Task<IActionResult> UpdateRole(Guid roleId, UpdateRoleModel updateRoleModel)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(roleId.ToString());

            role.Name = updateRoleModel.RoleName;

            var result = await _roleManager.UpdateAsync(role);

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

        [HttpDelete]
        [Route("{roleId}")]
        public async Task<IActionResult> DeleteRole(Guid roleId)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(roleId.ToString());

            if (role == null)
            {
                return NotFound(new { Message = "Role doen't exist" });
            }

            var result = await _roleManager.DeleteAsync(role);

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
    }
}
