﻿using Identity.Domain.Contracts;
using Identity.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebApi.Controllers;
[Route("api/auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("sign-in")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn(SignModel signModel, CancellationToken cancellationToken)
    {
        TokenModel tokenModel = await _authenticationService.SignInAsync(signModel, cancellationToken);
        return LingoMqResponses.LingoMqResponse.OkResult(tokenModel);
    }

    [HttpPost("sign-up")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp(SignModel signModel, CancellationToken cancellationToken)
    {
        int signUpStatus = await _authenticationService.SignUpAsync(signModel, cancellationToken);
        return LingoMqResponses.LingoMqResponse.AcceptedResult();
    }

    [HttpGet("refresh-token/{token}")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken(string token, CancellationToken cancellationToken)
    {
        TokenModel tokenModel = await _authenticationService.RefreshTokenAsync(token, cancellationToken);
        return LingoMqResponses.LingoMqResponse.OkResult(tokenModel);
    }
}
