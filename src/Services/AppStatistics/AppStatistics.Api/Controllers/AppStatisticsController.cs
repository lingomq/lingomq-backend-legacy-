using AppStatistics.BusinessLayer.Contracts.Service;
using AppStatistics.BusinessLayer.Exceptions.ClientExceptions;
using AppStatistics.DomainLayer.Entities;
using LingoMq.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace AppStatistics.Api.Controllers
{
    [Route("api/v0/app-statistics")]
    [ApiController]
    public class AppStatisticsController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IAppStatisticsService _appStatisticsService;
        public AppStatisticsController(IAppStatisticsService appStatisticsService) =>
            _appStatisticsService = appStatisticsService;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            List<StatisticsApp> statisticsList = await _appStatisticsService.GetAsync();
            StatisticsApp? statistics = statisticsList.Where(s => s.Date == DateTime.Now || s.Date == DateTime.Now.AddDays(-1))
                .FirstOrDefault();

            if (statistics is null)
                throw new NotFoundException<StatisticsApp>();

            _logger.Info("GET / {0}", nameof(List<StatisticsApp>));
            return LingoMqResponse.OkResult(statistics);
        }

        [HttpGet("{from}&{to}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(DateTime from, DateTime to)
        {
            List<StatisticsApp> statisticsList = await _appStatisticsService.GetByDateRange(from, to);
            _logger.Info("GET /{from}&{to} {0}", nameof(List<StatisticsApp>));
            return LingoMqResponse.OkResult(statisticsList);
        }
    }
}
