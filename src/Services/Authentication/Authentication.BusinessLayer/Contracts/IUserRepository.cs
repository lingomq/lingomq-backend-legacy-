using Authentication.DomainLayer.Entities;

namespace Authentication.BusinessLayer.Contracts
{
    public interface IUserRepository : IGenericRepository<User, User> 
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
