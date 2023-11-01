using AppStatistics.BusinessLayer.Contracts.DataAccess;
using AppStatistics.BusinessLayer.Contracts.Service;
using AppStatistics.BusinessLayer.Exceptions.ClientExceptions;
using AppStatistics.DomainLayer.Entities;

namespace AppStatistics.BusinessLayer.Services
{
    public class AppStatisticsService : IAppStatisticsService
    {
        private readonly IAppStatisticsDataAccessService _dataAccess;
        public AppStatisticsService(IAppStatisticsDataAccessService dataAccess) =>
            _dataAccess = dataAccess;

        public async Task CreateAsync(StatisticsApp entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await _dataAccess.CreateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            if (_dataAccess.GetAsync(id) is null)
                throw new NotFoundException<StatisticsApp>();

            await _dataAccess.DeleteAsync(id);
        }

        public async Task<StatisticsApp?> GetAsync(string id) =>
            await _dataAccess.GetAsync(id);

        public async Task<List<StatisticsApp>> GetAsync() =>
            await _dataAccess.GetAsync();

        public async Task<List<StatisticsApp>> GetByDateRange(DateTime start, DateTime finish) =>
            await _dataAccess.GetByDateRange(start, finish);

        public async Task UpdateAsync(StatisticsApp entity)
        {
            if (_dataAccess.GetAsync(entity.Id!) is null)
                throw new NotFoundException<StatisticsApp>();

            await _dataAccess.UpdateAsync(entity);
        }
    }
}
