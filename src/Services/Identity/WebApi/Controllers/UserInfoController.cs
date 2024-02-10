using Identity.Domain.Constants;
using Identity.Domain.Contracts;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.WebApi.Controllers;
[Route("api/identity/user/info")]
[ApiController]
public class UserInfoController : ControllerBase
{
    private readonly IUserInfoService _userInfoService;
    private Guid UserId => new Guid(User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
        .FirstOrDefault()?.Value!);
    public UserInfoController(IUserInfoService userInfoService)
    {
        _userInfoService = userInfoService;
    }

    [HttpGet]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get()
    {
        UserInfo info = await _userInfoService.GetByIdAsync(UserId);
        return LingoMqResponses.LingoMqResponse.OkResult(info);
    }

    [HttpGet("all/{range}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> GetRange(int range = int.MaxValue)
    {
        List<UserInfo> infos = await _userInfoService.GetRangeAsync(range);
        return LingoMqResponses.LingoMqResponse.OkResult(infos);
    }

    [HttpGet("user-id/{userId}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(Guid userId)
    {
        UserInfo info = await _userInfoService.GetByIdAsync(userId);
        return LingoMqResponses.LingoMqResponse.OkResult(info);
    }

    [HttpPut]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Update(UserInfo userInfo, CancellationToken cancellationToken)
    {
        await _userInfoService.UpdateAsync(userInfo, cancellationToken);
        return LingoMqResponses.LingoMqResponse.AcceptedResult();
    }
}
