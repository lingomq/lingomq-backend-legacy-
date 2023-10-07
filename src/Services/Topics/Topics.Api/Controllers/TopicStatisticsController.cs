using LingoMq.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Topics.Api.Common;
using Topics.BusinessLayer.Contracts;
using Topics.BusinessLayer.Dtos;
using Topics.BusinessLayer.Exceptions.ClientExceptions;
using Topics.BusinessLayer.Extensions;
using Topics.DomainLayer.Entities;

namespace Topics.Api.Controllers
{
    [Route("api/topic/statistics")]
    [ApiController]
    public class TopicStatisticsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public TopicStatisticsController(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        [HttpGet("all/{range}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(int range)
        {
            List<TopicStatistics> statistics = await _unitOfWork.TopicStatistics.GetAsync(range);
            return LingoMqResponse.OkResult(statistics);
        }

        [HttpGet("topic/{id}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> GetByTopicId(Guid id)
        {
            List<TopicStatistics> statistics = await _unitOfWork.TopicStatistics.GetByTopicIdAsync(id);
            return LingoMqResponse.OkResult(statistics);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(Guid id)
        {
            TopicStatistics? statistics = await _unitOfWork.TopicStatistics.GetByIdAsync(id);
            if (statistics is null)
                throw new NotFoundException<TopicStatistics>();

            return LingoMqResponse.OkResult(statistics);
        }

        [HttpPost]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Create(TopicStatisticsDto statistics)
        {
            if (!await CheckDoesNotExist(statistics))
                throw new ConflictException<TopicStatistics>();

            await _unitOfWork.TopicStatistics.AddAsync(statistics.ToModel());

            return LingoMqResponse.AcceptedResult(statistics);
        }

        [HttpPut]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> Update(TopicStatisticsDto statisticsDto)
        {
            if (await _unitOfWork.TopicStatistics.GetByIdAsync(statisticsDto.Id) is null)
                throw new InvalidDataException<TopicStatisticsDto>(new string[] { "Id" });

            await _unitOfWork.TopicStatistics.UpdateAsync(statisticsDto.ToModel());
            return LingoMqResponse.AcceptedResult(statisticsDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _unitOfWork.TopicStatistics.GetByIdAsync(id) is null)
                throw new InvalidDataException<TopicStatisticsDto>(new string[] { "Id" });

            await _unitOfWork.TopicStatistics.DeleteAsync(id);
            return LingoMqResponse.AcceptedResult();
        }

        private async Task<bool> CheckDoesNotExist(TopicStatisticsDto statistics)
        {
            List<TopicStatistics> statisticsFromDb = await _unitOfWork.TopicStatistics.GetByTopicIdAsync(statistics.TopicId);
            if (statisticsFromDb.Count == 0 || !statisticsFromDb.Any())
                return true;

            if (statisticsFromDb.Where(x => x.StatisticsTypeId == statistics.StatisticsTypeId).Count() == 0)
                return true;

            return false;
        }
    }
}
