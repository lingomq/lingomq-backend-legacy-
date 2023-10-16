using Microsoft.AspNetCore.Mvc;

namespace LingoMq.Responses;
/// <summary>
/// This class contains a generic responses for easy use them into 
/// the project that always returns some <c>IActionResult</c>
/// </summary>
/// <returns>IActionResult, depends on a caller member</returns>
public class ResponseResult : ControllerBase
{
    /// <summary>
    /// Returns a HTTP Information Status Code (100)
    /// </summary>
    /// <typeparam name="T">The response model</typeparam>
    /// <param name="code">The domain app status code which created owner</param>
    /// <returns>IActionResult</returns>
    public static IActionResult InformationResult<T>(int code) =>
        new ObjectResult(new { code }) { StatusCode = 100 };

    /// <summary>
    /// Returns a HTTP Success Status Code (200)
    /// </summary>
    /// <typeparam name="T">The response model</typeparam>
    /// <param name="code">The domain app status code which created owner</param>
    /// <param name="message">The app message</param>
    /// <param name="data">The response model</param>
    /// <returns>IActionResult</returns>
    public static IActionResult SuccessResult<T>(int code, string message, T data) =>
        new OkObjectResult(new { code, message, data });

    /// <summary>
    /// Returns a HTTP Success Status Code (200)
    /// </summary>
    /// <param name="code">The domain app status code which created owner</param>
    /// <param name="message">The app message</param>
    /// <returns>IActionResult</returns>
    public static IActionResult SuccessResult(int code, string message) =>
        new OkObjectResult(new { code, message });

    /// <summary>
    /// Returns a HTTP ClientError Status Code (400)
    /// </summary>
    /// <typeparam name="T">The response model</typeparam>
    /// <param name="code">The domain app status code which created owner</param>
    /// <param name="message">The app message</param>
    /// <param name="errors">The errors, which behold on runtime</param>
    /// <returns>IActionResult</returns>
    public static IActionResult ClientErrorResult<T>(int code, string message, T errors) =>
        new BadRequestObjectResult(new { code, message, errors});

    /// <summary>
    /// Returns a HTTP ClientError Status Code (400)
    /// </summary>
    /// <param name="code">The domain app status code which created owner</param>
    /// <param name="message">The app message</param>
    /// <returns>IActionResult</returns>
    public static IActionResult ClientErrorResult(int code, string message) =>
        new BadRequestObjectResult(new { code, message});

    /// <summary>
    /// Returns a HTTP ServerError Status Code (500)
    /// </summary>
    /// <param name="code">The domain app status code which created owner</param>
    /// <param name="message">The app message</param>
    /// <returns>IActionResult</returns>
    public static IActionResult ServerErrorResult(int code, string message) =>
        new ObjectResult(new { code, message }) { StatusCode = 500}; 
}

