﻿using Identity.Domain.Constants;
using Identity.Domain.Contracts;
using Identity.Domain.Entities;
using Identity.Domain.Exceptions.ClientExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.WebApi.Controllers;
[Route("api/identity/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private Guid UserId => new Guid(User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
            .FirstOrDefault()?.Value!);
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get()
    {
        User user = await _userService.GetByIdAsync(UserId);
        return LingoMqResponses.LingoMqResponse.OkResult(user);
    }

    [HttpGet("{userId}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(Guid userId)
    {
        User user = await _userService.GetByIdAsync(userId);
        return LingoMqResponses.LingoMqResponse.OkResult(user);
    }

    [HttpPut]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Update(User user, CancellationToken cancellationToken = default)
    {
        if (!user.Id.Equals(UserId))
            throw new ForbiddenException<User>("Вы не являетесь владельцем аккаунта");

        await _userService.UpdateAsync(user, cancellationToken);
        return LingoMqResponses.LingoMqResponse.AcceptedResult();
    }

    [HttpPut("credentials")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> UpdateCredentials(UserCredentialsModel credentialsModel, CancellationToken cancellationToken = default)
    {
        await _userService.UpdateCredentialsAsync(credentialsModel, cancellationToken);
        return LingoMqResponses.LingoMqResponse.AcceptedResult();
    }

    [HttpDelete]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken)
    {
        await _userService.DeleteByIdAsync(UserId, cancellationToken);
        return LingoMqResponses.LingoMqResponse.AcceptedResult();
    }

    [HttpDelete("{userId}")]
    [Authorize(Roles = AccessRoles.Admin)]
    public async Task<IActionResult> Delete(Guid userId, CancellationToken cancellationToken)
    {
        await _userService.DeleteByIdAsync(userId, cancellationToken);
        return LingoMqResponses.LingoMqResponse.AcceptedResult();
    }
}
