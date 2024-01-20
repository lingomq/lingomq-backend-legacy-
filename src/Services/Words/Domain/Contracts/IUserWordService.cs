using Words.Domain.Entities;
using Words.Domain.Enums;
using Words.Domain.Models;

namespace Words.Domain.Contracts;
public interface IUserWordService
{
    Task<List<UserWord>> GetByUserIdAsync(Guid id);
    Task<UserWord> GetMostRepeatedAsync(Guid userId);
    Task<List<RecordsModel>> GetRecordsAsync(RecordTypesEnum type, OrderingEnum ordering, int count);
    Task<UserWord> GetByIdAsync(Guid id);
    Task<int> GetAverageUserWordCountsAsync(Guid id, DateTime date);
    Task CreateAsync(UserWord userWord, bool isForce, bool isAutocomplete, Guid userId);
    Task AddRepeatsAsync(Guid id);
    Task DeleteAsync(Guid id);

}

