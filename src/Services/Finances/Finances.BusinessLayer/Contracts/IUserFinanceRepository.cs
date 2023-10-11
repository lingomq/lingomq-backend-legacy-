using Finances.DomainLayer.Entities;

namespace Finances.BusinessLayer.Contracts
{
    public interface IUserFinanceRepository : IGenericRepository<UserFinance>
    {
        Task<List<UserFinance?>> GetByUserId(Guid userId);
    }
}
