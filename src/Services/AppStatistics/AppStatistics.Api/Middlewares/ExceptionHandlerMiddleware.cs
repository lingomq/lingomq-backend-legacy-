using AppStatistics.BusinessLayer.Exceptions;
using AppStatistics.BusinessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using System.Net;

namespace AppStatistics.Api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly RequestDelegate _next;
        public ExceptionHandlerMiddleware(RequestDelegate next)
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
            catch (Exception ex)
            {

                ErrorModel model = new ErrorModel()
                {
                    Code = 0,
                    Message = ex.Message
                };

                _logger.Error("Type: {0}; Message: {1};", ex.Source, ex.Message);

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
    }
}
