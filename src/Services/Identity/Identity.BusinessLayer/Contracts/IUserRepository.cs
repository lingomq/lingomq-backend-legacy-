using Identity.DomainLayer.Entities;
using Identity.BusinessLayer.Dtos;

namespace Identity.BusinessLayer.Contracts
{
    public interface IUserRepository : IGenericRepository<User, UserDto>
    {
        Task<UserDto?> GetByEmailAsync(string email);
    }
}
