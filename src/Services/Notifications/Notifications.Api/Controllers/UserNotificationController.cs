using System.Security.Claims;
using LingoMq.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifications.Api.Common;
using Notifications.BusinessLayer.Contracts;
using Notifications.BusinessLayer.Exceptions.ClientExceptions;
using Notifications.DomainLayer.Entities;

namespace Notifications.Api.Controllers;

[Route("api/v0/notifications/user")]
[ApiController]
public class UserNotificationController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private Guid UserId => new(User.Claims
        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!);

    public UserNotificationController(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;

    [HttpGet]
    [Authorize(Roles = AccessRoles.All)]
    public async Task<IActionResult> Get()
    {
        List<UserNotification> notifications = await _unitOfWork.UserNotifications.GetByUserIdAsync(UserId);
        return LingoMqResponse.OkResult(notifications);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = AccessRoles.All)]
    public async Task<IActionResult> Update(Guid id)
    {
        UserNotification? notification = await _unitOfWork.UserNotifications.GetAsync(id);
        if (notification is null)
            throw new NotFoundException<UserNotification>();
        if (notification.UserId != UserId) throw new ForbiddenException<User>("Access denied");

        await _unitOfWork.UserNotifications.MarkAsReadAsync(id);
        return LingoMqResponse.AcceptedResult(notification);
    }
}