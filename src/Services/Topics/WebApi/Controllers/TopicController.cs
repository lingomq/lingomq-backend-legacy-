using LingoMq.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Topics.Domain.Constants;
using Topics.Domain.Contracts;
using Topics.Domain.Entities;
using Topics.Domain.Models;

namespace Topics.WebApi.Controllers;
[Route("api/topics")]
[ApiController]
public class TopicController : ControllerBase
{
    private readonly ITopicService _topicService;
    public TopicController(ITopicService topicService)
    {
        _topicService = topicService;
    }

    [HttpGet("all/skip/{skip}/take/{take}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(int skip = 0, int take = int.MaxValue)
    {
        List<Topic> topics = await _topicService.GetAsync(skip, take);
        return LingoMqResponse.OkResult(topics);
    }

    [HttpGet("filters")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> GetWithFilters(Guid? languageId, Guid? levelid, DateTime? startDate, DateTime? endDate, int skip = 0, int take = 100, string search = "")
    {
        if (startDate is null) startDate = DateTime.UnixEpoch;
        if (endDate is null) endDate = DateTime.Now;
        TopicFilters filters = new TopicFilters
        {
            LanguageId = languageId,
            LevelId = levelid,
            StartDate = startDate,
            EndDate = endDate,
            Skip = skip,
            Take = take,
            Search = search
        };

        List<Topic> topics = await _topicService.UseFilters(filters);
        return LingoMqResponse.OkResult(topics);
    }

    [HttpGet("topic-id/{id}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(Guid id)
    {
        Topic? topic = await _topicService.GetAsync(id);
        return LingoMqResponse.OkResult(topic);
    }

    [HttpPost]
    [Authorize(Roles = AccessRoles.Staff)]
    public async Task<IActionResult> Create(Topic topic)
    {
        await _topicService.CreateAsync(topic);
        return LingoMqResponse.AcceptedResult(topic);
    }

    [HttpPut]
    [Authorize(Roles = AccessRoles.Staff)]
    public async Task<IActionResult> Update(Topic topic)
    {
        await _topicService.UpdateAsync(topic);
        return LingoMqResponse.AcceptedResult(topic);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = AccessRoles.Staff)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _topicService.DeleteAsync(id); 
        return LingoMqResponse.AcceptedResult();
    }
}
