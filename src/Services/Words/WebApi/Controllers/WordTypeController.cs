using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Words.Domain.Constants;
using Words.Domain.Contracts;
using Words.Domain.Entities;

namespace Words.Api.Controllers;
[Route("api/words/word-types")]
[ApiController]
public class WordTypeController : ControllerBase
{
    private readonly IWordTypeService _wordTypeService;
    public WordTypeController(IWordTypeService wordTypeService)
    {
        _wordTypeService = wordTypeService;
    }

    [HttpGet("all/{range}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(int range = int.MaxValue)
    {
        List<WordType> wordTypes = await _wordTypeService.GetRangeAsync(range);
        return LingoMq.Responses.LingoMqResponse.OkResult(wordTypes);
    }

    [HttpGet("id/{id}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(Guid id)
    {
        WordType wordType = await _wordTypeService.GetAsync(id);
        return LingoMq.Responses.LingoMqResponse.OkResult(wordType);
    }

    [HttpPost]
    [Authorize(Roles = AccessRoles.Staff)]
    public async Task<IActionResult> Create(WordType wordType)
    {
        await _wordTypeService.CreateAsync(wordType);
        return LingoMq.Responses.LingoMqResponse.AcceptedResult();
    }

    [HttpPut]
    [Authorize(Roles = AccessRoles.Staff)]
    public async Task<IActionResult> Update(WordType wordType)
    {
        await _wordTypeService.UpdateAsync(wordType);
        return LingoMq.Responses.LingoMqResponse.AcceptedResult();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = AccessRoles.Staff)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _wordTypeService.DeleteAsync(id);
        return LingoMq.Responses.LingoMqResponse.AcceptedResult();
    }
}
