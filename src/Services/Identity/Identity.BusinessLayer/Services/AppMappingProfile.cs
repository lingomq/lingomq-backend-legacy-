using AutoMapper;
using Identity.BusinessLayer.Dtos;
using Identity.DomainLayer.Entities;

namespace Identity.BusinessLayer.Services
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
