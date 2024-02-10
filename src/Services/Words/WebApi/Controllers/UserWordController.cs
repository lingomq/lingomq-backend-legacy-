using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Words.Domain.Constants;
using Words.Domain.Contracts;
using Words.Domain.Entities;
using Words.Domain.Enums;
using Words.Domain.Models;

namespace Words.Api.Controllers;
[Route("api/words/user-words")]
[ApiController]
public class UserWordController : ControllerBase
{
    private readonly IUserWordService _userWordService;
    private Guid UserId => new Guid(User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
            .FirstOrDefault()?.Value!);
    public UserWordController(IUserWordService userWordService)
    {
        _userWordService = userWordService;
    }

    [HttpGet("user")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get()
    {
        List<UserWord> words = await _userWordService.GetByUserIdAsync(UserId);
        return LingoMqResponses.LingoMqResponse.OkResult(words);
    }

    [HttpGet("user/{userId}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(Guid userId)
    {
        List<UserWord> words = await _userWordService.GetByUserIdAsync(userId);
        return LingoMqResponses.LingoMqResponse.OkResult(words);
    }

    [HttpGet("famous")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> GetMostRepeatedWord()
    {
        UserWord userWord = await _userWordService.GetMostRepeatedAsync(UserId);
        return LingoMqResponses.LingoMqResponse.OkResult(userWord);
    }

    [HttpGet("records/type/{type}/order/{order}/count/{count}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> GetRecords(string type = "word-count", string order = "ASC", int count = 4)
    {
        RecordTypesEnum recordType = RecordTypesEnum.None;
        OrderingEnum ordering = order == "DESC" ? OrderingEnum.DESC : OrderingEnum.ASC;

        if (type == "word-count")
            recordType = RecordTypesEnum.Words;
        else recordType = RecordTypesEnum.Repeats;

        List<RecordsModel> records = await _userWordService.GetRecordsAsync(recordType, ordering, count);
        return LingoMqResponses.LingoMqResponse.OkResult(records);
    }

    [HttpGet("word/count/{userId}/{date}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> GetAverageUserWordCounts(Guid userId, DateTime dateTime)
    {
        int count = await _userWordService.GetAverageUserWordCountsAsync(userId, dateTime);
        return LingoMqResponses.LingoMqResponse.OkResult(new { Count = count });
    }

    [HttpPost("{isForce}/{isAutocomplete}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Create([FromBody] UserWord word, bool isForce = false, bool isAutocomplete = false)
    {
        await _userWordService.CreateAsync(word, isForce, isAutocomplete, UserId);
        return LingoMqResponses.LingoMqResponse.AcceptedResult();
    }

    [HttpPut("add/repeats/{wordId}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> AddRepeats(Guid wordId)
    {
        await _userWordService.AddRepeatsAsync(wordId);
        return LingoMqResponses.LingoMqResponse.AcceptedResult();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userWordService.DeleteAsync(id);
        return LingoMqResponses.LingoMqResponse.AcceptedResult();
    }
}

