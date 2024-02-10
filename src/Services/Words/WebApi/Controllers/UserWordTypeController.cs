using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Words.Domain.Constants;
using Words.Domain.Contracts;
using Words.Domain.Entities;

namespace Words.Api.Controllers;
[Route("api/words/user-word-types")]
[ApiController]
public class UserWordTypeController : ControllerBase
{
    private readonly IUserWordTypeService _userWordTypeService;
    private Guid UserId => new Guid(User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
        .FirstOrDefault()?.Value!);
    public UserWordTypeController(IUserWordTypeService userWordTypeService)
    {
        _userWordTypeService = userWordTypeService;
    }

    [HttpGet("all/{range}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(int range)
    {
        List<UserWordType> userWordTypes = await _userWordTypeService.GetRangeAsync(range);
        return LingoMqResponses.LingoMqResponse.OkResult(userWordTypes);
    }

    [HttpGet("word-id/{wordId}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> GetByWordId(Guid id)
    {
        List<UserWordType> userWordTypes = await _userWordTypeService.GetByWordIdAsync(id);
        return LingoMqResponses.LingoMqResponse.OkResult(userWordTypes);
    }

    [HttpGet("type-id/{typeId}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> GetByTopicId(Guid id)
    {
        List<UserWordType> userWordTypes = await _userWordTypeService.GetByTypeIdAsync(id);
        return LingoMqResponses.LingoMqResponse.OkResult(userWordTypes);
    }

    [HttpPost]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Create(UserWordType userWordType)
    {
        await _userWordTypeService.CreateAsync(userWordType);
        return LingoMqResponses.LingoMqResponse.AcceptedResult();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userWordTypeService.DeleteAsync(id);
        return LingoMqResponses.LingoMqResponse.AcceptedResult();
    }
}
