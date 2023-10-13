using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TFA.Domain.Authorization;
using TFA.Domain.Exceptions;

namespace TFA.API.Midddlewares
{
    public static class ProblemDetailsFactoryExtensions
    {
        public static ProblemDetails CreateFrom(
            this ProblemDetailsFactory factory,
            HttpContext context,
            IntentionManagerException exception)
        {
            return factory.CreateProblemDetails(context, StatusCodes.Status403Forbidden, "Authorization failed",
                detail: exception.Message);
        }

        public static ProblemDetails CreateFrom(
            this ProblemDetailsFactory factory,
            HttpContext context,
            DomainException exception)
        {
            return factory.CreateProblemDetails(context, exception.ErrorCode switch
            {
                DomainErrorCode.Gone => StatusCodes.Status410Gone,
                _ => StatusCodes.Status500InternalServerError
            },
            detail: exception.Message);
        }

        public static ProblemDetails CreateFrom(
            this ProblemDetailsFactory factory,
            HttpContext context,
            ValidationException validationException)
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in validationException.Errors)
            {
                modelStateDictionary.AddModelError(error.PropertyName, error.ErrorCode);
            }

            return factory.CreateValidationProblemDetails(
                context,
                modelStateDictionary,
                StatusCodes.Status400BadRequest,
                "Validation failed");
        }
    }
}
