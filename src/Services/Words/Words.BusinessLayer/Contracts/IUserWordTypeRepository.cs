using Words.DomainLayer.Entities;

namespace Words.BusinessLayer.Contracts
{
    public interface IUserWordTypeRepository : IGenericRepository<UserWordType>
    {
        Task<UserWordType?> GetByWordId(Guid id);
    }
}
