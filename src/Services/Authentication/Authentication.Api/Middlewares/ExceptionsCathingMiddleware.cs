using Authentication.BusinessLayer.Exceptions;
using Authentication.BusinessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Authentication.Api.Middlewares
{
    public class ExceptionsCathingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionsCathingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ClientExceptionBase ex)
            {
                if (ex.Result is not null)
                    await HandleCustomExceptionAsync(context, ex);
                else
                    await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleCustomExceptionAsync(HttpContext context, ClientExceptionBase exceptionBase)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            await exceptionBase.Result!.ExecuteResultAsync(new ActionContext
            {
                HttpContext = context
            });
        }
        private async Task HandleExceptionAsync(HttpContext context, ClientExceptionBase exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            ErrorModel model = new ErrorModel()
            {
                Code = exception.Code,
                Message = exception.Message
            };

            var result = JsonConvert.SerializeObject(model);

            await context.Response.WriteAsync(result);
        }
    }
}
