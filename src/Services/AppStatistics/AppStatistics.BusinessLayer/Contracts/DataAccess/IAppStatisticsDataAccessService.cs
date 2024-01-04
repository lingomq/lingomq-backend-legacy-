using AppStatistics.DomainLayer.Entities;

namespace AppStatistics.BusinessLayer.Contracts.DataAccess
{
    public interface IAppStatisticsDataAccessService : IGenericDataAccessService<StatisticsApp>
    {
        Task<List<StatisticsApp>> GetByDateRange(DateTime start, DateTime finish);
    }
}
