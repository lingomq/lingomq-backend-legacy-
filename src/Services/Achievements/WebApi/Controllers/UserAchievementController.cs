using Achievements.Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Achievements.Domain.Constants;
using Achievements.Domain.Entities;
using System.Security.Claims;
using LingoMq.Responses;

namespace Achievements.WebApi.Controllers;
[Route("api/achievements/user")]
[ApiController]
public class UserAchievementController : ControllerBase
{
    private readonly IUserAchievementService _userAchievementService;
    public UserAchievementController(IUserAchievementService userAchievementService) =>
        _userAchievementService = userAchievementService;
    private Guid UserId => new Guid(User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
        .FirstOrDefault()?.Value!);

    [HttpGet]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get()
    {
        List<UserAchievement> achievements = await _userAchievementService.GetAsync(UserId);
        return LingoMqResponse.OkResult(achievements);
    }

    [HttpGet("{userId}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(Guid userId)
    {
        List<UserAchievement> achievements = await _userAchievementService.GetAsync(UserId);
        return LingoMqResponse.OkResult(achievements);
    }

    [HttpGet("count/{userId}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> GetAchievemensCount(Guid userId)
    {
        int count = await _userAchievementService.GetAchievementsCount(userId);
        return LingoMqResponse.OkResult(new { Count = count });
    }
}
