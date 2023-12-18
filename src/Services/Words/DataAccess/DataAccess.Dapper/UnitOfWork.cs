using DataAccess.Dapper.Contracts;

namespace DataAccess.Dapper;
public class UnitOfWork : IUnitOfWork
{
    public ILanguageRepository Languages { get; }

    public IUserRepository Users { get; }

    public IUserWordRepository UserWords { get; }

    public IUserWordTypeRepository UserWordTypes { get; }

    public IWordTypeRepository WordTypes { get; }
    public UnitOfWork(ILanguageRepository languages, IUserRepository users,
        IUserWordRepository userWords, IUserWordTypeRepository userWordTypes,
        IWordTypeRepository wordTypes)
    {
        Languages = languages;
        Users = users;
        UserWords = userWords;
        UserWordTypes = userWordTypes;
        WordTypes = wordTypes;
    }

}
