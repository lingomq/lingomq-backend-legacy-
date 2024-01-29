using Finances.BusinessLayer.Contracts;

namespace Finances.BusinessLayer.Services;

public class UnitOfWork : IUnitOfWork
{
    public IFinanceRepository Finances { get; }
    public IUserFinanceRepository UserFinances { get; }

    public UnitOfWork(IFinanceRepository financeRepository,
        IUserFinanceRepository userFinanceRepository)
    {
        Finances = financeRepository;
        UserFinances = userFinanceRepository;
    }
}