using System;
using System.Net;
using Domain.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Domain.WebApi.Filters.ExceptionHandlers
{
    sealed class DomainRuleExceptionHandler : IExceptionHandler
    {
        public (ProblemDetails error, HttpStatusCode statusCode)? Handle(Exception exception, HttpContext httpContext)
        {
            if (!(exception is DomainRuleException domainRuleException))
            {
                return null;
            }

            var type = domainRuleException.GetEntityType();
            var problemDetails = new ProblemDetails
            {
                Title = type?.Name,
                Detail = domainRuleException.Message
            };
            foreach (var (key, value) in exception.GetErrorDetailFields())
            {
                problemDetails.Extensions.Add(key, value);
            }

            return (problemDetails, HttpStatusCode.UnprocessableEntity);
        }
    }
}
