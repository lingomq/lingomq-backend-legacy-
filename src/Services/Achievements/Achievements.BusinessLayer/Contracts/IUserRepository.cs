using Achievements.DomainLayer.Entities;

namespace Achievements.BusinessLayer.Contracts
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    } 
}
