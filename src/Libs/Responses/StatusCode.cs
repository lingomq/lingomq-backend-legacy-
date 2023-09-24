using Microsoft.AspNetCore.Mvc;
using Responses.StatusCodes;

namespace Responses;

public class StatusCode
{
    public static IActionResult OkResult<T>(T data, string message = "Success") => 
        ResponseResult.SuccessResult<T>((int) SuccessCodes.Success, message, data);

    public static IActionResult AcceptedResult<T>(T data, string message = "Accepted") =>
        ResponseResult.SuccessResult<T>((int) SuccessCodes.Accepted, message, data);

    public static IActionResult NoContentResult<T>(T data, string message = "No Content") =>
        ResponseResult.SuccessResult<T>((int) SuccessCodes.NoContent, message, data);

    public static IActionResult BadRequestResult<T>(T error, string message = "An error occurred") =>
        ResponseResult.ClientErrorResult<T>((int) ClientErrorCodes.BadRequest, message, error);
    
    public static IActionResult NotFoundResult<T>(T error, string message = "Object not found. See error for additional info") =>
        ResponseResult.ClientErrorResult((int) ClientErrorCodes.NotFound, message, error);
    
    public static IActionResult RequestTimeoutResult() =>
        ResponseResult.ClientErrorResult((int) ClientErrorCodes.Timeout, "Request timeout. Check your connection");

    public static IActionResult ConflictResult<T>(T error, string message = "Conflict") =>
        ResponseResult.ClientErrorResult((int) ClientErrorCodes.Conflict, message, error);

    public static IActionResult InternalServerError(string message = "Internal server error") =>
        ResponseResult.ServerErrorResult(0, message);
}


