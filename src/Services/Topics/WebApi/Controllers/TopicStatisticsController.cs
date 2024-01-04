using LingoMq.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Topics.Domain.Constants;
using Topics.Domain.Contracts;
using Topics.Domain.Entities;

namespace Topics.WebApi.Controllers;

[Route("api/topic/statistics")]
[ApiController]
public class TopicStatisticsController : ControllerBase
{
    private readonly ITopicStatisticsService _topicStatisticsService;
    public TopicStatisticsController(ITopicStatisticsService topicStatisticsService)
    {
        _topicStatisticsService = topicStatisticsService;
    }

    [HttpGet("all/{range}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(int range)
    {
        List<TopicStatistics> statistics = await _topicStatisticsService.GetAsync(range);
        return LingoMqResponse.OkResult(statistics);
    }

    [HttpGet("topic/{id}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> GetByTopicId(Guid id)
    {
        List<TopicStatistics> statistics = await _topicStatisticsService.GetByTopicIdAsync(id);
        return LingoMqResponse.OkResult(statistics);
    }

    [HttpGet("statistics-id/{id}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(Guid id)
    {
        TopicStatistics? statistics = await _topicStatisticsService.GetAsync(id);
        return LingoMqResponse.OkResult(statistics);
    }

    [HttpPost]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Create(TopicStatistics statistics)
    {
        await _topicStatisticsService.CreateAsync(statistics);
        return LingoMqResponse.AcceptedResult(statistics);
    }

    [HttpPut]
    [Authorize(Roles = AccessRoles.Admin)]
    public async Task<IActionResult> Update(TopicStatistics statistics)
    {
        await _topicStatisticsService.UpdateAsync(statistics);
        return LingoMqResponse.AcceptedResult(statistics);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = AccessRoles.Admin)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _topicStatisticsService.DeleteAsync(id);
        return LingoMqResponse.AcceptedResult();
    }
}
