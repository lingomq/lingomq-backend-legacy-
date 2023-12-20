using Achievements.Domain.Constants;
using Achievements.Domain.Contracts;
using Achievements.Domain.Entities;
using LingoMq.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Achievements.Api.Controllers;

[Route("api/achievements")]
[ApiController]
public class AchievementController : ControllerBase
{
    private readonly IAchievementService _achievementService;
    public AchievementController(IAchievementService achievementService) =>
        _achievementService = achievementService;

    [HttpGet("all/{range}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(int range)
    {
        List<Achievement> achievements = await _achievementService.GetAsync(range);
        return LingoMqResponse.OkResult(achievements);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(Guid id)
    {
        Achievement? achievement = await _achievementService.GetAsync(id);
        return LingoMqResponse.OkResult(achievement);
    }

    [HttpPost]
    [Authorize(Roles = AccessRoles.Admin)]
    public async Task<IActionResult> Create(Achievement achievement)
    {
        await _achievementService.CreateAsync(achievement);
        return LingoMqResponse.AcceptedResult(achievement);
    }

    [HttpPut]
    [Authorize(Roles = AccessRoles.Admin)]
    public async Task<IActionResult> Update(Achievement achievement)
    {
        await _achievementService.UpdateAsync(achievement);
        return LingoMqResponse.AcceptedResult(achievement);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = AccessRoles.Admin)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _achievementService.DeleteAsync(id);
        return LingoMqResponse.AcceptedResult();
    }
}
