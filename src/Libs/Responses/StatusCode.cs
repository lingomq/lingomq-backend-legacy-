using Microsoft.AspNetCore.Mvc;
using Responses.StatusCodes;

namespace Responses;

public class StatusCode
{
    public static IActionResult OkResult<T>(T data, string message = "Success") =>
        ResponseResult.SuccessResult((int)SuccessCodes.Success, message, data);

    public static IActionResult AcceptedResult<T>(T data, string message = "Accepted") =>
        ResponseResult.SuccessResult((int)SuccessCodes.Accepted, message, data);

    public static IActionResult NoContentResult<T>(T data, string message = "No Content") =>
        ResponseResult.SuccessResult((int)SuccessCodes.NoContent, message, data);

    public static IActionResult BadRequestResult<T>(T error, string message = "An error occurred") =>
        ResponseResult.ClientErrorResult((int)ClientErrorCodes.BadRequest, message, error);

    public static IActionResult ForbiddenResult<T>(T error, string message = "Forbidden") =>
        ResponseResult.ClientErrorResult((int)ClientErrorCodes.Forbidden, message, error);

    public static IActionResult NotFoundResult(string message = "Object not found. See error for additional info") =>
        ResponseResult.ClientErrorResult((int)ClientErrorCodes.NotFound, message);

    public static IActionResult UnauthorizedResult(string message = "Unauthorized") =>
        ResponseResult.ClientErrorResult((int)ClientErrorCodes.NotFound, message);

    public static IActionResult UnauthorizedResult<T>(T error, string message = "Unauthorized") =>
        ResponseResult.ClientErrorResult((int)ClientErrorCodes.NotFound, message, error);

    public static IActionResult NotFoundResult<T>(T error, string message = "Object not found. See error for additional info") =>
        ResponseResult.ClientErrorResult((int)ClientErrorCodes.NotFound, message, error);

    public static IActionResult RequestTimeoutResult() =>
        ResponseResult.ClientErrorResult((int)ClientErrorCodes.Timeout, "Request timeout. Check your connection");

    public static IActionResult ConflictResult<T>(T error, string message = "Conflict") =>
        ResponseResult.ClientErrorResult((int)ClientErrorCodes.Conflict, message, error);

    public static IActionResult InternalServerError(string message = "Internal server error") =>
        ResponseResult.ServerErrorResult(0, message);
}


