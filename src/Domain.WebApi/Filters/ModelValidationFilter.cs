using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Domain.WebApi.Filters
{
    public sealed class ModelValidationFilter : IActionFilter
    {
        readonly ILogger logger;

        public ModelValidationFilter(ILogger logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            var problemDetails = new ValidationProblemDetails(context.ModelState);
            context.Result = new BadRequestObjectResult(problemDetails);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (!(context.Result is OkObjectResult result))
            {
                return;
            }

            var dto = result.Value;
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(dto, validationContext, validationResults))
            {
                logger.Warning("Validation of response object failed {Action} {ValidationResults}", context.ActionDescriptor.DisplayName, validationResults);
                context.Result = new ObjectResult(validationResults) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }
    }
}