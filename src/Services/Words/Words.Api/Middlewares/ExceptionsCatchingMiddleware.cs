using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using System.Net;
using Words.BusinessLayer.Exceptions;
using Words.BusinessLayer.Models;

namespace Words.Api.Middlewares
{
    /// <summary>
    /// This class <c>ExceptionHandlerMiddleware</c> is a class which handle a lot of 
    /// program exceptions without sending this summary to user
    /// </summary>
    public class ExceptionsCatchingMiddleware
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly RequestDelegate _next;
        public ExceptionsCatchingMiddleware(RequestDelegate next)
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

                _logger.Warn("Type: {0}; Message: {1};", ex.Source, ex.Message);
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
    }
}
