using System.Data;
using Achievements.DataAccess.Dapper.Contracts;
using Achievements.DataAccess.Dapper.Postgres.RawQueries;
using Achievements.DataAccess.Dapper.Utils;
using Achievements.Domain.Entities;

namespace Achievements.DataAccess.Dapper.Postgres.Realizations;
public class AchievementRepository : GenericRepository<Achievement>, IAchievementRepository
{
    public AchievementRepository(IDbConnection connection) : base(connection)
    {
    }

    public async Task<Achievement?> GetByIdAsync(Guid id) =>
        await QueryFirstAsync(AchievementQueries.GetById, new { Id = id });

    public async Task<List<Achievement>> GetAsync(int range) =>
        await QueryListAsync(AchievementQueries.GetRange, new { Count = range });

    public async Task AddAsync(Achievement entity) =>
        await ExecuteByTemplateAsync(AchievementQueries.Create, entity);

    public async Task UpdateAsync(Achievement entity) =>
        await ExecuteByTemplateAsync(AchievementQueries.Update, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(AchievementQueries.Delete, new { Id = id });
}
