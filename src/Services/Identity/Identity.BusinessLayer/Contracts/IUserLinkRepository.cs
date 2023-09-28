using Identity.DomainLayer.Entities;

namespace Identity.BusinessLayer.Contracts
{
    public interface IUserLinkRepository : IGenericRepository<UserLink, UserLink>
    {
        Task<UserLink?> GetByUserId(Guid id);
        Task<UserLink?> GetByInfoId(Guid id);
    }
}
