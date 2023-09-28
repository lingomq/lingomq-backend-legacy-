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
    }
}
