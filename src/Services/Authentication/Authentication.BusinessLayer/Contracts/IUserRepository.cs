using Authentication.DomainLayer.Entities;

namespace Authentication.BusinessLayer.Contracts
{
    public interface IUserRepository : IGenericRepository<User, User> 
    {
        Task<User?> GetByNickname(string nickname);
        Task<User?> GetByEmail(string email);
    }
}
