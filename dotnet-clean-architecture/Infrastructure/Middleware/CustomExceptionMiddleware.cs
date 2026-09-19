using Application.Common.Exceptions;
using Application.Common.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;
        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exceptionObj)
            {
                await HandleExceptionAsync(context, exceptionObj, _logger);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<CustomExceptionMiddleware> logger)
        {
            _logger.LogError("Start Logging The Error");
            _logger.LogError("------------------------");

            _logger.LogError($"The Actual Error Details Are {exception.Message}");

            string defaultMessage = exception.Message;

            //if (_webHost.IsProduction())
            //{
            //    defaultMessage = _appResourceService.GetResource("Error_Occured_Try_later");
            //}

            var result = new ResultOfAction<object>(HttpStatusCode.InternalServerError, defaultMessage, null);

            if (exception is CustomHttpException)
            {
                var customHttpException = (CustomHttpException)exception;

                result.StatusCode = customHttpException.StatusCode;
                result.Message = customHttpException.Message;
            }

            context.Response.StatusCode = result.StatusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));

            _logger.LogError("------------------------");
            _logger.LogError("End Logging The Error");
        }
    }
}
