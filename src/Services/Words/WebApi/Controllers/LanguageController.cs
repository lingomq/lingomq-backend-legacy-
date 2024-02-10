using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Words.Domain.Constants;
using Words.Domain.Contracts;
using Words.Domain.Entities;

namespace Words.Api.Controllers;
[Route("api/words/languages")]
[ApiController]
public class LanguageController : ControllerBase
{
    private readonly ILanguageService _languageService;
    public LanguageController(ILanguageService languageService)
    {
        _languageService = languageService;
    }

    [HttpGet("all/{range}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(int range = int.MaxValue)
    {
        List<Language> languages = await _languageService.GetAsync(range);
        return LingoMqResponses.LingoMqResponse.OkResult(languages);
    }

    [HttpGet("id/{id}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(Guid id)
    {
        Language language = await _languageService.GetAsync(id);
        return LingoMqResponses.LingoMqResponse.OkResult(language);
    }

    [HttpPost]
    [Authorize(Roles = AccessRoles.Admin)]
    public async Task<IActionResult> Create(Language language)
    {
        await _languageService.CreateAsync(language);
        return LingoMqResponses.LingoMqResponse.AcceptedResult();
    }

    [HttpPut]
    [Authorize(Roles = AccessRoles.Admin)]
    public async Task<IActionResult> Update(Language language)
    {
        await _languageService.UpdateAsync(language);
        return LingoMqResponses.LingoMqResponse.AcceptedResult();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = AccessRoles.Admin)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _languageService.DeleteAsync(id);
        return LingoMqResponses.LingoMqResponse.AcceptedResult();
    }
}
