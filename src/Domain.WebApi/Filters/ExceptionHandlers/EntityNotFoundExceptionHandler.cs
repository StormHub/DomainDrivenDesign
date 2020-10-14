using System;
using System.Net;
using Domain.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Domain.WebApi.Filters.ExceptionHandlers
{
    sealed class EntityNotFoundExceptionHandler : IExceptionHandler
    {
        public (ProblemDetails error, HttpStatusCode statusCode)? Handle(Exception exception, HttpContext httpContext)
        {
            if (!(exception is EntityNotFoundException)) return null;

            var entityType = exception.GetEntityType();
            var problemDetails = new ProblemDetails
            {
                Type = entityType.Name
            };
            foreach (var (key, value) in exception.GetErrorDetailFields())
            {
                problemDetails.Extensions.Add(key, value);
            }
            
            return (problemDetails, HttpStatusCode.NotFound);
        }
    }
}
