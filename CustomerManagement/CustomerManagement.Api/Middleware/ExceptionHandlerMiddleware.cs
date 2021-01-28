using System;
using System.Net;
using System.Threading.Tasks;
using CustomerManagement.Api.Common.Exception;
using CustomerManagement.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Api.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogger<ExceptionHandlerMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) when (ex is CustomException)
            {
                await HandleExceptionAsync(context, ex, logger);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<ExceptionHandlerMiddleware> logger)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            if (ex is UpdateEntityRuleException || ex is UniqueEntityRuleException)
                code = HttpStatusCode.UnprocessableEntity;
            else if (ex is BadRequestException)
                code = HttpStatusCode.BadRequest;
            else if (ex is NotFoundException) code = HttpStatusCode.NotFound;

            if (code == HttpStatusCode.InternalServerError)
                logger.LogError(ex, ex.Message);
            else
                logger.LogInformation(ex.Message);

            context.Response.Clear();

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            return context.Response.WriteAsync(new ApiError
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            }.ToString());
        }
    }
}