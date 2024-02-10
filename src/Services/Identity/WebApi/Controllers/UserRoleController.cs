using Identity.Domain.Constants;
using Identity.Domain.Contracts;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserRoleController : ControllerBase
{
    private readonly IUserRoleService _userRoleService;
    public UserRoleController(IUserRoleService userRoleService)
    {
        _userRoleService = userRoleService;
    }

    [HttpGet("all/{range}")]
    [Authorize(Roles = AccessRoles.Staff)]
    public async Task<IActionResult> Get(int range = int.MaxValue)
    {
        List<UserRole> roles = await _userRoleService.GetRangeAsync(range);
        return LingoMqResponses.LingoMqResponse.OkResult(roles);
    }

    [HttpGet("id/{id}")]
    [Authorize(Roles = AccessRoles.Staff)]
    public async Task<IActionResult> Get(Guid id)
    {
        UserRole role = await _userRoleService.GetByIdAsync(id);
        return LingoMqResponses.LingoMqResponse.OkResult(role);
    }

    [HttpPost]
    [Authorize(Roles = AccessRoles.Admin)]
    public async Task<IActionResult> Create(UserRole role, CancellationToken cancellationToken)
    {
        await _userRoleService.CreateAsync(role, cancellationToken);
        return LingoMqResponses.LingoMqResponse.AcceptedResult();
    }

    [HttpPut]
    [Authorize(Roles = AccessRoles.Admin)]
    public async Task<IActionResult> Update(UserRole role, CancellationToken cancellationToken)
    {
        await _userRoleService.UpdateAsync(role, cancellationToken);
        return LingoMqResponses.LingoMqResponse.AcceptedResult();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = AccessRoles.Admin)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _userRoleService.DeleteAsync(id, cancellationToken);
        return LingoMqResponses.LingoMqResponse.AcceptedResult();
    }
}
