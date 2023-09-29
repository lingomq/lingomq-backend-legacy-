using Identity.BusinessLayer.Dtos;
using Identity.DomainLayer.Entities;

namespace Identity.BusinessLayer.Extensions
{
    public static class UserExtensions
    {
        public static UserDto ToDto(this User user) =>
            new UserDto()
            {
                Id = user.Id,
                Email = user.Email,
                Phone = user.Phone
            };
        public static User ToUserModel(this UserDto userDto) =>
            new User()
            {
                Id = userDto.Id,
                Email = userDto.Email,
                Phone = userDto.Phone
            };

    }
}
