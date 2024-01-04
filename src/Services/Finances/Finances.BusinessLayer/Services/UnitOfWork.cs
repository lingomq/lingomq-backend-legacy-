using Finances.BusinessLayer.Contracts;

namespace Finances.BusinessLayer.Services;

public class UnitOfWork : IUnitOfWork
{
    public IFinanceRepository Finances { get; }
    public IUserFinanceRepository UserFinances { get; }
    public IUserRepository Users { get; }

    public UnitOfWork(IFinanceRepository financeRepository,
        IUserFinanceRepository userFinanceRepository,
        IUserRepository userRepository)
    {
        Finances = financeRepository;
        UserFinances = userFinanceRepository;
        Users = userRepository;
    }
}