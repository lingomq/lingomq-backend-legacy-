using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifications.Domain.Constants;
using Notifications.Domain.Contracts;
using Notifications.Domain.Entities;
using System.Security.Claims;

namespace Notifications.WebApi.Controllers;
[Route("api/notifications/user")]
[ApiController]
public class UserNotificationController : ControllerBase
{
    private readonly IUserNotificationService _userNotificationService;
    private Guid UserId => new(User.Claims
        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!);

    public UserNotificationController(IUserNotificationService userNotificationService) =>
        _userNotificationService = userNotificationService;

    [HttpGet]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get()
    {
        List<UserNotification> notifications = await _userNotificationService.GetAsync(UserId);
        return LingoMqResponses.LingoMqResponse.OkResult(notifications);
    }

    [HttpPut("id/{id}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Update(Guid id)
    {
        await _userNotificationService.UpdateAsync(id);
        return LingoMqResponses.LingoMqResponse.AcceptedResult();
    }
}
