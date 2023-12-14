using Words.BusinessLayer.Models;
using Words.DomainLayer.Entities;

namespace Words.BusinessLayer.Contracts
{
    public interface IUserWordRepository : IGenericRepository<UserWord>
    {
        Task<int> GetCountWordsPerDayAsync(Guid id, DateTime day);
        Task<UserWord?> GetByWordAsync(string word);
        Task<List<UserWord>> GetByUserIdAsync(Guid id);
        Task<UserWord?> GetMostRepeatedWordAsync(Guid userId);
        Task<bool> AddRepeatsAsync(Guid wordId, int count);
        Task<List<RecordsByRepeatsResponseModel>> GetRecordsByRepeatsAsync(int count, string order);
        Task<List<RecordsByWordsCountResponseModel>> GetRecordsByWordsCountsAsync(int count, string order);
    }
}
