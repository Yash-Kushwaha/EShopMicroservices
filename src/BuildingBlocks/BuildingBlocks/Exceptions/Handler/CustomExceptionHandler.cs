using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BuildingBlocks.Exceptions.Handler
{
    public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError("Error Message : {exceptioMessage}, Time of occurrence : {timeOfOccurrence}", exception.Message, DateTime.UtcNow);
            (string Detail, string Title, int StatusCOde) = exception switch
            {
                InternalServerException =>
                (
                    Detail: exception.Message,
                    Title: exception.GetType().Name,
                    StatusCOde: StatusCodes.Status500InternalServerError
                ),
                ValidationException =>
                (
                    Detail: exception.Message,
                    Title: exception.GetType().Name,
                    StatusCOde: StatusCodes.Status400BadRequest
                ),
                BadRequestException =>
                (
                    Detail: exception.Message,
                    Title: exception.GetType().Name,
                    StatusCOde: StatusCodes.Status400BadRequest
                ),
                NotFoundException =>
                (
                    Detail: exception.Message,
                    Title: exception.GetType().Name,
                    StatusCOde: StatusCodes.Status404NotFound
                ),
                _ =>
                (
                    Detail: "An unexpected error occurred.",
                    Title: "InternalServerError",
                    StatusCOde: StatusCodes.Status500InternalServerError
                )
            };

            var problemDetails = new ProblemDetails
            {
                Title = Title,
                Status = StatusCOde,
                Detail = Detail,
                Instance = context.Request.Path
            };

            problemDetails.Extensions["traceId"] = context.TraceIdentifier;

            if(exception is ValidationException validationException)
            {
                var errors = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
                problemDetails.Extensions["errors"] = errors;
            }

            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
