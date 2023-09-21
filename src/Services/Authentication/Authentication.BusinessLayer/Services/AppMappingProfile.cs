using Authentication.BusinessLayer.Dtos;
using Authentication.DomainLayer.Entities;
using AutoMapper;

namespace Authentication.BusinessLayer.Services
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile() 
        {
            CreateMap<UserInfoDto, UserInfo>();
        }
    }
}
