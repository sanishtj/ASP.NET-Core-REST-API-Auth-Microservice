using AuthDataAccess.Abstractions;
using AuthDataAccess.Entities;
using AuthMicroservice.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AuthMicroservice.Controllers
{

    [Route("api/tenants")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class TenantsController : ControllerBase
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        ILogger<TenantsController> _log;

        public TenantsController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TenantsController> log)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _log = log;
        }
        [HttpGet]
        [Authorize]
        public ActionResult<List<TenantModel>> Get()
        {
            List<Tenant> tenants = _unitOfWork.TenantRepository.GetTenants();

            var tenantsModel = _mapper.Map<List<TenantModel>>(tenants);

            return Ok(tenantsModel);
        }

        [HttpGet]
        [Route("{tenantId}", Name = "GetATenant")]
        public ActionResult<TenantModel> Get(Guid tenantId)
        {
            Tenant tenant = _unitOfWork.TenantRepository.GetTenant(tenantId);

            var tenantModel = _mapper.Map<TenantModel>(tenant);

            return Ok(tenantModel);
        }

        [HttpPost]
        public ActionResult<TenantModel> Post(TenantCreationModel tenantCreationModel)
        {
            try
            {
                Tenant tenant = _mapper.Map<Tenant>(tenantCreationModel);

                _unitOfWork.TenantRepository.CreateTenant(tenant);
                _unitOfWork.Commit();

                TenantModel createdTenantModel = _mapper.Map<TenantModel>(tenant);
                return CreatedAtRoute("GetATenant", new { tenantId = createdTenantModel.TenantId }, createdTenantModel);
            }
            catch (Exception ex)
            {
                _log.LogError("api/tenants : Post TraceId : " + Activity.Current?.Id, ex);
                throw;
            }
        }

        [HttpPatch]
        [Route("{tenantId}")]
        public ActionResult Update(Guid tenantId, JsonPatchDocument<TenantUpdateModel> patchdocument)
        {
            // Check whether Tenant exists
            if (!_unitOfWork.TenantRepository.TenantExists(tenantId))
            {
                return NotFound();
            }

            var tenant = _unitOfWork.TenantRepository.GetTenant(tenantId);

            var tenantToPatch = _mapper.Map<TenantUpdateModel>(tenant);

            patchdocument.ApplyTo(tenantToPatch, ModelState);

            if (!TryValidateModel(tenantToPatch))
                return ValidationProblem(ModelState);

            _mapper.Map(tenantToPatch, tenant);

            _unitOfWork.Commit();

            return NoContent();
        }

        [HttpDelete]
        [Route("{tenantId}")]
        public ActionResult Delete(Guid tenantId)
        {
            // Check whether Tenant exists
            if (!_unitOfWork.TenantRepository.TenantExists(tenantId))
            {
                return NotFound();
            }

            _unitOfWork.TenantRepository.DeleteTenant(tenantId);
            _unitOfWork.Commit();

            return NoContent();
        }



    }
}
