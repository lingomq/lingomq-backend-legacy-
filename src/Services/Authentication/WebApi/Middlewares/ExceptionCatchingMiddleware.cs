using Authentication.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Authentication.WebApi.Middlewares;

public class ExceptionCatchingMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionCatchingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ExceptionBase ex)
        {
            if (ex.Result is not null)
                await HandleCustomExceptionAsync(context, ex);
            else
            {
                ErrorModel model = new ErrorModel()
                {
                    Code = ex.Code,
                    Message = ex.Message
                };
                await HandleAsync(context, (int)ex.ExceptionStatusCode, model);
            }
        }
        catch (Exception ex)
        {

            ErrorModel model = new ErrorModel()
            {
                Code = 0,
            };

            Console.WriteLine(ex.Source);
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);

            await HandleAsync(context, (int)HttpStatusCode.InternalServerError, model);
        }
    }

    private async Task HandleCustomExceptionAsync(HttpContext context, ExceptionBase exceptionBase)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)exceptionBase.ExceptionStatusCode;

        await exceptionBase.Result!.ExecuteResultAsync(new ActionContext
        {
            HttpContext = context
        });
    }
    private async Task HandleAsync(HttpContext context, int code, ErrorModel model)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = code;

        var result = JsonConvert.SerializeObject(model);

        await context.Response.WriteAsync(result);
    }

    private class ErrorModel
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string? Message { get; set; }
        public override string ToString()
        {
            return Code + " " + Message;
        }
    }
}
