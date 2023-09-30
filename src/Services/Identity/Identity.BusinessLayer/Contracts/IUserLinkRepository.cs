using Identity.DomainLayer.Entities;

namespace Identity.BusinessLayer.Contracts
{
    public interface IUserLinkRepository : IGenericRepository<UserLink, UserLink>
    {
        Task<List<UserLink>> GetAllByIdAsync(Guid id);
        Task<List<UserLink>> GetByUserInfoIdAsync(Guid id);
    }
}
