using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Domain.WebApi.Filters.ExceptionHandlers
{
    public interface IExceptionHandler
    {
        (ProblemDetails error, HttpStatusCode statusCode)? Handle(Exception exception, HttpContext httpContext);
    }
}
