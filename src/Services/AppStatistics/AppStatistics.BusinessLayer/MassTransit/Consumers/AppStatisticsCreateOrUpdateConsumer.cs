using AppStatistics.BusinessLayer.Contracts.Service;
using AppStatistics.DomainLayer.Entities;
using EventBus.Entities.AppStatistics;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AppStatistics.BusinessLayer.MassTransit.Consumers
{
    public class AppStatisticsCreateOrUpdateConsumer : IConsumer<AppStatisticsCreateOrUpdate>
    {
        private readonly ILogger<AppStatisticsCreateOrUpdateConsumer> _logger;
        private readonly IAppStatisticsService _service;
        public AppStatisticsCreateOrUpdateConsumer(ILogger<AppStatisticsCreateOrUpdateConsumer> logger,
            IAppStatisticsService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task Consume(ConsumeContext<AppStatisticsCreateOrUpdate> context)
        {
            var yesterdayStatisticsList = await _service.GetByDateRange(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1));
            StatisticsApp? yesterdayStatistics = yesterdayStatisticsList.FirstOrDefault();

            if (yesterdayStatistics is null)
                yesterdayStatistics = new StatisticsApp() { TotalUsers = 0, TotalWords = 0, Downloads = 0 };

            var todayStatisticsList = await _service.GetByDateRange(context.Message.Date, context.Message.Date);
            StatisticsApp? todayStatistics = todayStatisticsList.FirstOrDefault();

            if (todayStatistics is null)
            {
                todayStatistics = new StatisticsApp()
                {
                    Id = Guid.NewGuid().ToString(),
                    TotalWords = yesterdayStatistics.TotalWords + context.Message.TotalWords,
                    TotalUsers = yesterdayStatistics.TotalUsers + context.Message.TotalUsers,
                    Downloads = yesterdayStatistics.Downloads + context.Message.Downloads
                };
                await _service.CreateAsync(todayStatistics);
            }
            else
            {
                todayStatistics = new StatisticsApp()
                {
                    Id = Guid.NewGuid().ToString(),
                    TotalWords = todayStatistics.TotalWords + context.Message.TotalWords,
                    TotalUsers = todayStatistics.TotalUsers + context.Message.TotalUsers,
                    Downloads = todayStatistics.Downloads + context.Message.Downloads
                };
                await _service.UpdateAsync(todayStatistics);
            }

            _logger.LogInformation("[+] [AppStatistics Consumer] UpdateStatisticsData. TotalUsers: {0}, TotalWords: {1}" ,
                todayStatistics.TotalUsers, todayStatistics.TotalWords);
        }
    }
}
