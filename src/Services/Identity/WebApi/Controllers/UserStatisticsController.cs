using Identity.Domain.Constants;
using Identity.Domain.Contracts;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.WebApi.Controllers;
[Route("api/identity/user/statistics")]
[ApiController]
public class UserStatisticsController : ControllerBase
{
    private readonly IUserStatisticsService _userStatisticsService;
    private Guid UserId => new Guid(User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
        .FirstOrDefault()?.Value!);

    public UserStatisticsController(IUserStatisticsService userStatisticsService)
    {
        _userStatisticsService = userStatisticsService;
    }

    [HttpGet("id/{userId}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(Guid userId)
    {
        UserStatistics statistics = await _userStatisticsService.GetByIdAsync(userId);
        return LingoMq.Responses.LingoMqResponse.OkResult(statistics);
    }

    [HttpPut("hour/add")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> AddHour()
    {
        await _userStatisticsService.AddHourToStatisticsAsync(UserId);
        return LingoMq.Responses.LingoMqResponse.AcceptedResult();
    }

    [HttpPut("word/add")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> AddWord()
    {
        await _userStatisticsService.AddWordToStatisticsAsync(UserId);
        return LingoMq.Responses.LingoMqResponse.AcceptedResult();
    }

    [HttpPut("visit")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> UpdateVisitStreak()
    {
        await _userStatisticsService.AddVisitationToStatisticsAsync(UserId);
        return LingoMq.Responses.LingoMqResponse.AcceptedResult();
    }
}
