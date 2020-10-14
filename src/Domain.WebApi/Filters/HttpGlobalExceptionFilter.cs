using Domain.WebApi.Filters.ExceptionHandlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Domain.WebApi.Filters
{
    public sealed class HttpGlobalExceptionFilter : IExceptionFilter
    {
        readonly IHttpErrorBuilder httpExceptionHandler;

        public HttpGlobalExceptionFilter(IHttpErrorBuilder httpExceptionHandler)
        {
            this.httpExceptionHandler = httpExceptionHandler;
        }

        public void OnException(ExceptionContext context)
        {
            var (errorDetail, statusCode) = httpExceptionHandler.Build(context.Exception, context.HttpContext);
            context.Result = new ObjectResult(errorDetail)
            {
                StatusCode = (int)statusCode
            };
            context.ExceptionHandled = true;
        }
    }
}
