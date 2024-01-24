using Identity.Domain.Contracts;
using Identity.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebApi.Controllers;
[Route("api/auth/confirm")]
[ApiController]
public class ConfirmationController : ControllerBase
{
    private readonly IConfirmationService _confirmationService;
    public ConfirmationController(IConfirmationService confirmationService)
    {
        _confirmationService = confirmationService;
    }

    [HttpGet("{token}")]
    public async Task<IActionResult> ConfirmEmail(string token, CancellationToken cancellationToken)
    {
        TokenModel tokenModel = await _confirmationService.ConfirmEmailAsync(token, cancellationToken);
        return LingoMq.Responses.LingoMqResponse.OkResult(tokenModel);
    }
}
