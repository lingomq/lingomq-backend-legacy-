using Microsoft.AspNetCore.Mvc;

namespace Responses;

public class StatusCode
{
    public static IActionResult OkResult<T>(T data, string message = "Success") => 
        ResponseResult.SuccessResult<T>(0, message, data);

    public static IActionResult AcceptedResult<T>(T data, string message = "Accepted") =>
        ResponseResult.SuccessResult<T>(1, message, data);

    public static IActionResult NoContentResult<T>(T data, string message = "No Content") =>
        ResponseResult.SuccessResult<T>(4, message, data);

    public static IActionResult BadRequestResult<T>(T error, string message = "An error occurred") =>
        ResponseResult.ClientErrorResult<T>(0, message, error);
    
    public static IActionResult NotFoundResult<T>(T error, string message = "Object not found. See error for additional info") =>
        ResponseResult.ClientErrorResult(4, message, error);
    
    public static IActionResult RequestTimeoutResult() =>
        ResponseResult.ClientErrorResult(8, "Request timeout. Check your connection");

    public static IActionResult ConflictResult(T error, string message = "Conflict") =>
        ResponseResult.ClientErrorResult(9, message, error);

    public static IActionResult InternalServerError(string message = "Internal server error") =>
        ResponseResult.ServerErrorResult(0, message);
}


