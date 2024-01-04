namespace Finances.BusinessLayer.Contracts
{
    public interface IUnitOfWork
    {
        IFinanceRepository Finances { get; }
        IUserFinanceRepository UserFinances { get; }
        IUserRepository Users { get; }
    }
}
