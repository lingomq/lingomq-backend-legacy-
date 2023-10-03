using Words.DomainLayer.Entities;

namespace Words.BusinessLayer.Contracts
{
    public interface IUserWordRepository : IGenericRepository<UserWord>
    {
        Task<int> GetCountWordsPerDayAsync(Guid id, DateTime day);
        Task<UserWord?> GetByWordAsync(string word);
        Task<UserWord?> GetMostRepeatedWordAsync();
    }
}
