using Microsoft.AspNetCore.Mvc;

namespace Responses;

public class ResponseResult : ControllerBase
{
    public static IActionResult InformationResult<T>(int code) =>
        new ObjectResult(new { code }) { StatusCode = 100 };
    
    public static IActionResult SuccessResult<T>(int code, string message, T data) =>
        new OkObjectResult(new { code, message, data });
    
    public static IActionResult ClientErrorResult<T>(int code, string message, T errors) =>
        new BadRequestObjectResult(new { code, message, errors});

    public static IActionResult ClientErrorResult(int code, string message) =>
        new BadRequestObjectResult(new { code, message});

    public static IActionResult ServerErrorResult(int code, string message) =>
        new ObjectResult(new { code, message }) { StatusCode = 500}; 
}

