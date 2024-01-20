using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifications.Domain.Constants;
using Notifications.Domain.Contracts;
using Notifications.Domain.Entities;

namespace Notifications.WebApi.Controllers;
[Route("api/notifications/type")]
[ApiController]
public class NotificationTypeController : ControllerBase
{
    private readonly INotificationService _notificationService;
    public NotificationTypeController(INotificationService notificationService) =>
        _notificationService = notificationService;

    [HttpGet("all/{range}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int range = int.MaxValue)
    {
        List<NotificationType> types = await _notificationService.GetAsync(range);
        return LingoMq.Responses.LingoMqResponse.OkResult(types);
    }

    [HttpGet("id/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(Guid id)
    {
        NotificationType? type = await _notificationService.GetAsync(id);
        return LingoMq.Responses.LingoMqResponse.OkResult(type);
    }

    [HttpPost]
    [Authorize(Roles = AccessRoles.Admin)]
    public async Task<IActionResult> Create(NotificationType type)
    {
        await _notificationService.CreateAsync(type);
        return LingoMq.Responses.LingoMqResponse.AcceptedResult(type);
    }

    [HttpPut]
    [Authorize(Roles = AccessRoles.Admin)]
    public async Task<IActionResult> Update(NotificationType type)
    {
        await _notificationService.UpdateAsync(type);
        return LingoMq.Responses.LingoMqResponse.AcceptedResult(type);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = AccessRoles.Admin)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _notificationService.DeleteAsync(id);
        return LingoMq.Responses.LingoMqResponse.AcceptedResult();
    }
}
