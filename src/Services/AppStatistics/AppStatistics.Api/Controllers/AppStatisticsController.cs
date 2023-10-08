using AppStatistics.BusinessLayer.Contracts.Service;
using AppStatistics.BusinessLayer.Exceptions.ClientExceptions;
using AppStatistics.DomainLayer.Entities;
using LingoMq.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppStatistics.Api.Controllers
{
    [Route("api/app-statistics")]
    [ApiController]
    public class AppStatisticsController : ControllerBase
    {
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

            return LingoMqResponse.OkResult(statistics);
        }

        [HttpGet("{from}&{to}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(DateTime from, DateTime to)
        {
            List<StatisticsApp> statisticsList = await _appStatisticsService.GetByDateRange(from, to);
            return LingoMqResponse.OkResult(statisticsList);
        }
    }
}
