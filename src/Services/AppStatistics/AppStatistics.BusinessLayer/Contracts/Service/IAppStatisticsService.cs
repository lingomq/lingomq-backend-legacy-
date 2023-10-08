using AppStatistics.DomainLayer.Entities;

namespace AppStatistics.BusinessLayer.Contracts.Service
{
    public interface IAppStatisticsService : IGenericService<StatisticsApp>
    {
        Task<List<StatisticsApp>> GetByDateRange(DateTime start, DateTime finish);
    }
}
