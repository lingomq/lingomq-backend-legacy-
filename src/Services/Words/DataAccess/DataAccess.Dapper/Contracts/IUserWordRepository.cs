using Words.Domain.Entities;
using Words.Domain.Enums;
using Words.Domain.Models;

namespace DataAccess.Dapper.Contracts;
public interface IUserWordRepository : IGenericRepository<UserWord>
{
    Task<int> GetCountWordsPerDayAsync(Guid id, DateTime day);
    Task<UserWord?> GetByWordAsync(string word);
    Task<List<UserWord>> GetByUserIdAsync(Guid id);
    Task<UserWord?> GetMostRepeatedWordAsync(Guid userId);
    Task AddRepeatsAsync(Guid wordId, int count);
    Task<List<RecordsModel>> GetRecordsAsync(RecordTypesEnum recordType, OrderingEnum ordering, int count);
}
