using Words.DomainLayer.Entities;

namespace Words.BusinessLayer.Contracts
{
    public interface IUserWordTypeRepository : IGenericRepository<UserWordType>
    {
        Task<List<UserWordType>> GetByUserIdAsync(Guid id);
        Task<List<UserWordType>> GetByTypeIdAsync(Guid id);
    }
}
