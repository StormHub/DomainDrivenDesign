using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Domain.WebApi.Filters.ExceptionHandlers
{
    public sealed class HttpErrorBuilder : IHttpErrorBuilder
    {
        readonly IReadOnlyList<IExceptionHandler> exceptionHandlers;
        readonly IWebHostEnvironment environment;
        readonly ILogger logger;

        public HttpErrorBuilder(IEnumerable<IExceptionHandler> exceptionHandlers, IWebHostEnvironment environment, ILogger logger)
        {
            this.exceptionHandlers = new List<IExceptionHandler>(exceptionHandlers);
            this.environment = environment;
            this.logger = logger;
        }

        public (ProblemDetails error, HttpStatusCode statusCode) Build(Exception exception, HttpContext context)
        {
            var result = Handle(exception, context);
            if (environment.IsDevelopment())
            {
                result.error.Extensions.Add(nameof(Exception.StackTrace), exception.StackTrace);
            }

            return result;
        }

        (ProblemDetails error, HttpStatusCode statusCode) Handle(Exception exception, HttpContext context)
        { 
            foreach (var handler in exceptionHandlers)
            {
                var result = handler.Handle(exception, context);
                if (result.HasValue)
                {
                    return result.Value;
                }
            }

            // Default unknown
            var displayUrl = context.Request.GetDisplayUrl();
            logger.Error(exception, "{Request}", displayUrl);

            var problemDetails = new ProblemDetails
            {
                Type = "Unexpected error"
            };
            if (environment.IsDevelopment())
            {
                problemDetails.Detail = exception.Message;
            }

            return (problemDetails, HttpStatusCode.InternalServerError);
        }
    }
}
