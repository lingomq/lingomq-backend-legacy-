using Microsoft.AspNetCore.Mvc;
using Responses.StatusCodes;

namespace LingoMq.Responses;

/// <summary>
/// The class is a wrapper on the <c>ResponseResult</c> class
/// Returns a realized result
/// <returns>IActionResult</returns>
/// </summary>
public class LingoMqResponse
{
    /// <summary>
    /// Returns a HTTP Success Status Code (200)
    /// </summary>
    /// <typeparam name="T">The response model</typeparam>
    /// <param name="data">The response model</param>
    /// <param name="message">The returns message status</param>
    /// <returns>IActionResult</returns>
    public static IActionResult OkResult<T>(T data, string message = "Success") =>
        ResponseResult.SuccessResult((int)SuccessCodes.Success, message, data);

    /// <summary>
    /// Returns a HTTP Information Status Code (100)
    /// </summary>
    /// <typeparam name="T">The response model</typeparam>
    /// <param name="data">The response model</param>
    /// <param name="message">The returns message status</param>
    /// <returns>IActionResult</returns>
    public static IActionResult AcceptedResult<T>(T data, string message = "Accepted") =>
        ResponseResult.SuccessResult((int)SuccessCodes.Accepted, message, data);

    /// <summary>
    /// Returns a HTTP Information Status Code (100)
    /// </summary>
    /// <param name="message">The returns message status</param>
    /// <returns>IActionResult</returns>
    public static IActionResult AcceptedResult(string message = "Accepted") =>
        ResponseResult.SuccessResult((int)SuccessCodes.Accepted, message);

    /// <summary>
    /// Returns a HTTP Success Status Code (201) with code NoContent (1)
    /// </summary>
    /// <typeparam name="T">The response model</typeparam>
    /// <param name="data">The response model</param>
    /// <param name="message">The returns message status</param>
    /// <returns>IActionResult</returns>
    public static IActionResult NoContentResult<T>(T data, string message = "No Content") =>
        ResponseResult.SuccessResult((int)SuccessCodes.NoContent, message, data);

    /// <summary>
    /// Returns a HTTP ClientError Status Code (400)
    /// </summary>
    /// <typeparam name="T">The response model</typeparam>
    /// <param name="error">The response model</param>
    /// <param name="message">The returns message status</param>
    /// <returns>IActionResult</returns>
    public static IActionResult BadRequestResult<T>(T error, string message = "An error occurred") =>
        ResponseResult.ClientErrorResult((int)ClientErrorCodes.BadRequest, message, error);

    /// <summary>
    /// Returns a HTTP ClientError Status Code (400) with code Forbidden (3)
    /// </summary>
    /// <typeparam name="T">The response model</typeparam>
    /// <param name="error">The response model</param>
    /// <param name="message">The returns message status</param>
    /// <returns>IActionResult</returns>
    public static IActionResult ForbiddenResult<T>(T error, string message = "Forbidden") =>
        ResponseResult.ClientErrorResult((int)ClientErrorCodes.Forbidden, message, error);

    /// <summary>
    /// Returns a HTTP ClientError Status Code (400) with code NotFound (4)
    /// </summary>
    /// <param name="error">The response model</param>
    /// <param name="message">The returns message status</param>
    /// <returns>IActionResult</returns>
    public static IActionResult NotFoundResult(string message = "Object not found. See error for additional info") =>
        ResponseResult.ClientErrorResult((int)ClientErrorCodes.NotFound, message);

    /// <summary>
    /// Returns a HTTP ClientError Status Code (400) with code Unauthorized (1)
    /// </summary>
    /// <param name="message">The returns message status</param>
    /// <returns>IActionResult</returns>
    public static IActionResult UnauthorizedResult(string message = "Unauthorized") =>
        ResponseResult.ClientErrorResult((int)ClientErrorCodes.NotFound, message);

    /// <summary>
    /// Returns a HTTP ClientError Status Code (400) with code Unauthorized (1)
    /// </summary>
    /// <typeparam name="T">The response model</typeparam>
    /// <param name="error">The response model</param>
    /// <param name="message">The returns message status</param>
    /// <returns>IActionResult</returns>
    public static IActionResult UnauthorizedResult<T>(T error, string message = "Unauthorized") =>
        ResponseResult.ClientErrorResult((int)ClientErrorCodes.NotFound, message, error);

    /// <summary>
    /// Returns a HTTP ClientError Status Code (400) with code NotFound (4)
    /// </summary>
    /// <typeparam name="T">The response model</typeparam>
    /// <param name="error">The response model</param>
    /// <param name="message">The returns message status</param>
    /// <returns>IActionResult</returns>
    public static IActionResult NotFoundResult<T>(T error, string message = "Object not found. See error for additional info") =>
        ResponseResult.ClientErrorResult((int)ClientErrorCodes.NotFound, message, error);

    /// <summary>
    /// Returns a HTTP ClientError Status Code (400) with code RequestTimeout (8)
    /// </summary>
    /// <returns>IActionResult</returns>
    public static IActionResult RequestTimeoutResult() =>
        ResponseResult.ClientErrorResult((int)ClientErrorCodes.Timeout, "Request timeout. Check your connection");

    /// <summary>
    /// Returns a HTTP ClientError Status Code (400) with code Conflict (9)
    /// </summary>
    /// <typeparam name="T">The response model</typeparam>
    /// <param name="error">The response model</param>
    /// <param name="message">The returns message status</param>
    /// <returns>IActionResult</returns>
    public static IActionResult ConflictResult<T>(T error, string message = "Conflict") =>
        ResponseResult.ClientErrorResult((int)ClientErrorCodes.Conflict, message, error);

    /// <summary>
    /// Returns a HTTP ServerError Status Code (500)
    /// </summary>
    /// <returns>IActionResult</returns>
    public static IActionResult InternalServerError(string message = "Internal server error") =>
        ResponseResult.ServerErrorResult(0, message);
}


