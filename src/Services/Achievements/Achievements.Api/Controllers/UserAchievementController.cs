using System.Security.Claims;
using Achievements.Domain.Constants;
using Achievements.Domain.Contracts;
using Achievements.Domain.Entities;
using LingoMq.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Achievements.Api.Controllers;
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
        List<UserAchievement> achievements = await _userAchievementService.GetByUserId(UserId);
        return LingoMqResponse.OkResult(achievements);
    }

    [HttpGet("user/{userId}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(Guid userId)
    {
        List<UserAchievement> achievements = await _userAchievementService.GetByUserId(userId);
        return LingoMqResponse.OkResult(achievements);
    }

    [HttpGet("count/{userId}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> GetAchievemensCount(Guid userId)
    {
        int count = await _userAchievementService.GetAchievementsCount(userId);
        return LingoMqResponse.OkResult(new { Count = count });
    }

    [HttpGet("all/{count}")]
    [Authorize(Roles = AccessRoles.Staff)]
    public async Task<IActionResult> GetAll(int count = int.MaxValue)
    {
        List<UserAchievement> userAchievements = await _userAchievementService.GetAsync(count);
        return LingoMqResponse.OkResult(userAchievements);
    }

    [HttpGet("id/{id}")]
    [Authorize(Roles = AccessRoles.Staff)]
    public async Task<IActionResult> GetById(Guid id)
    {
        UserAchievement? userAchievement = await _userAchievementService.GetAsync(id);
        return LingoMqResponse.OkResult(userAchievement);
    }
}
