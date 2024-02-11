using Application.Services.UserWordActions;
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
        return LingoMq.Responses.LingoMqResponse.OkResult(words);
    }

    [HttpGet("user/{userId}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Get(Guid userId)
    {
        List<UserWord> words = await _userWordService.GetByUserIdAsync(userId);
        return LingoMq.Responses.LingoMqResponse.OkResult(words);
    }

    [HttpGet("famous")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> GetMostRepeatedWord()
    {
        UserWord userWord = await _userWordService.GetMostRepeatedAsync(UserId);
        return LingoMq.Responses.LingoMqResponse.OkResult(userWord);
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
        return LingoMq.Responses.LingoMqResponse.OkResult(records);
    }

    [HttpGet("word/count/{userId}/{date}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> GetAverageUserWordCounts(Guid userId, DateTime dateTime)
    {
        int count = await _userWordService.GetAverageUserWordCountsAsync(userId, dateTime);
        return LingoMq.Responses.LingoMqResponse.OkResult(new { Count = count });
    }
    /*
        [HttpPost("{isForce}/{isAutocomplete}")]
        [Authorize(Roles = AccessRoles.Everyone)]
        public async Task<IActionResult> Create([FromBody] UserWord word, bool isForce = false, bool isAutocomplete = false)
        {
            await _userWordService.CreateAsync(word, isForce, isAutocomplete, UserId);
            return LingoMq.Responses.LingoMqResponse.AcceptedResult();
        }*/

    [HttpPost("append/word/{wordId}/user/{userId}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Create(Guid wordId, Guid userId)
    {
        await _userWordService.CreateAsync(wordId, userId);
        return LingoMq.Responses.LingoMqResponse.AcceptedResult();
    }

    [HttpPut("add/repeats/{wordId}/user/{userId}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> AddRepeats(Guid wordId, Guid userId)
    {
        await _userWordService.AddRepeatsAsync(userId, wordId);
        return LingoMq.Responses.LingoMqResponse.AcceptedResult();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = AccessRoles.Everyone)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userWordService.DeleteAsync(id);
        return LingoMq.Responses.LingoMqResponse.AcceptedResult();
    }
}

