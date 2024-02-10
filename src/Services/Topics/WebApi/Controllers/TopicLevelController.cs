using LingoMqResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Topics.Domain.Constants;
using Topics.Domain.Contracts;
using Topics.Domain.Entities;

namespace Topics.WebApi.Controllers;

[Route("api/topics/level")]
[ApiController]
public class TopicLevelController : ControllerBase
{
    private readonly ITopicLevelService _topicLevelService;
    public TopicLevelController(ITopicLevelService topicLevelService)
    {
        _topicLevelService = topicLevelService;
    }

    [HttpGet("all/{range}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(int range = int.MaxValue)
    {
        List<TopicLevel> levels = await _topicLevelService.GetAsync(range);
        return LingoMqResponse.OkResult(levels);
    }

    [HttpGet("topic-id/{id}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(Guid id)
    {
        TopicLevel level = await _topicLevelService.GetAsync(id);
        return LingoMqResponse.OkResult(level);
    }

    [HttpPost]
    [Authorize(Roles = AccessRoles.Staff)]
    public async Task<IActionResult> Create(TopicLevel level)
    {
        await _topicLevelService.CreateAsync(level);
        return LingoMqResponse.AcceptedResult(level);
    }

    [HttpPut]
    [Authorize(Roles = AccessRoles.Staff)]
    public async Task<IActionResult> Update(TopicLevel level)
    {
        await _topicLevelService.UpdateAsync(level);
        return LingoMqResponse.AcceptedResult(level);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = AccessRoles.Staff)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _topicLevelService.DeleteAsync(id);
        return LingoMqResponse.AcceptedResult();
    }
}
