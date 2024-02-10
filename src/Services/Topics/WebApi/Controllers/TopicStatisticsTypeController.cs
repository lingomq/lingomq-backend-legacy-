using LingoMqResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Topics.Domain.Constants;
using Topics.Domain.Contracts;
using Topics.Domain.Entities;

namespace Topics.WebApi.Controllers;

[Route("api/topics/statistics/types")]
[ApiController]
public class TopicStatisticsTypeController : ControllerBase
{
    private readonly ITopicStatisticsTypeService _topicStatisticsTypeService;
    public TopicStatisticsTypeController(ITopicStatisticsTypeService topicStatisticsTypeService)
    {
        _topicStatisticsTypeService = topicStatisticsTypeService;
    }

    [HttpGet("all/{range}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(int range = int.MaxValue)
    {
        List<TopicStatisticsType> types = await _topicStatisticsTypeService.GetAsync(range);
        return LingoMqResponse.OkResult(types);
    }

    [HttpPost]
    [Authorize(Roles = AccessRoles.Staff)]
    public async Task<IActionResult> Create(TopicStatisticsType type)
    {
        await _topicStatisticsTypeService.CreateAsync(type);
        return LingoMqResponse.AcceptedResult(type);
    }

    [HttpPut]
    [Authorize(Roles = AccessRoles.Staff)]
    public async Task<IActionResult> Update(TopicStatisticsType type)
    {
        await _topicStatisticsTypeService.UpdateAsync(type);
        return LingoMqResponse.AcceptedResult(type);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = AccessRoles.Staff)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _topicStatisticsTypeService.DeleteAsync(id);
        return LingoMqResponse.AcceptedResult();
    }
}
