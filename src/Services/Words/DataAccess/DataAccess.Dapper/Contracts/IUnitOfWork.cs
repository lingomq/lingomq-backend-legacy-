namespace DataAccess.Dapper.Contracts;
public interface IUnitOfWork
{
    public ILanguageRepository Languages { get; }
    public IUserRepository Users { get; }
    public IUserWordRepository UserWords { get; }
    public IWordTypeRepository WordTypes { get; }
}
