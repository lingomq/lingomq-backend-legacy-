using AppStatistics.BusinessLayer.Contracts.DataAccess;
using AppStatistics.DomainLayer.Entities;
using MongoDB.Driver;

namespace AppStatistics.BusinessLayer.Services.DataAccess
{
    public class AppStatisticsDataAccessService : IAppStatisticsDataAccessService
    {
        private readonly IMongoCollection<StatisticsApp> _items;
        public AppStatisticsDataAccessService(IMongoDbFactory factory) =>
            _items = factory.GetCollection<StatisticsApp>("lingomq_statistics", "app_statistics");

        public async Task CreateAsync(StatisticsApp entity) =>
            await _items.InsertOneAsync(entity);

        public async Task DeleteAsync(string id) =>
            await _items.DeleteOneAsync(x => x.Id == id);

        public async Task<StatisticsApp?> GetAsync(string id)
        {
            IAsyncCursor<StatisticsApp> statistics = await _items.FindAsync(x => x.Id == id);
            return await statistics.FirstOrDefaultAsync();
        }

        public async Task<List<StatisticsApp>> GetByDateRange(DateTime start, DateTime finish)
        {
            IAsyncCursor<StatisticsApp> statistics = await _items.FindAsync(x => x.Date >= start && x.Date <= finish);
            return await statistics.ToListAsync();
        }

        public async Task<List<StatisticsApp>> GetAsync()
        {
            IAsyncCursor<StatisticsApp> statistics = await _items.FindAsync(x => true);
            return await statistics.ToListAsync();
        }

        public async Task UpdateAsync(StatisticsApp entity) =>
            await _items.FindOneAndReplaceAsync(x => x.Id == entity.Id, entity);
    }
}
