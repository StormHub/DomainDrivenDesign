using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Domain.WebApi.Filters.ExceptionHandlers
{
    public interface IHttpErrorBuilder
    {
        public (ProblemDetails error, HttpStatusCode statusCode) Build(Exception exception, HttpContext context);

    }
}
