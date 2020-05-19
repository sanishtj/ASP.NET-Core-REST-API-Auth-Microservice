using AuthDataAccess.Entities;
using AuthMicroservice.Models;
using AutoMapper;

namespace AuthMicroservice.Profiles
{
    public class TenantProfile : Profile
    {
        public TenantProfile()
        {
            CreateMap<Tenant, TenantModel>();
            CreateMap<TenantCreationModel, Tenant>();
            CreateMap<Tenant, TenantUpdateModel>();
            CreateMap<TenantUpdateModel, Tenant>();
        }
    }
}
