using Words.DomainLayer.Entities;

namespace Words.BusinessLayer.Contracts
{
    public interface IUserWordRepository : IGenericRepository<UserWord>
    {
        Task<int> GetCountWordsPerDay(Guid id, DateTime day);
        Task<UserWord?> GetByWord(string word);
        Task<UserWord?> GetMostRepeatedWord();
    }
}
