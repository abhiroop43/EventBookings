using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger, IWebHostEnvironment environment)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError("Exception: {0} Occurred On: {1}", exception.Message, DateTime.UtcNow);

        (string Detail, string Title, int StatusCode) detail = exception switch
        {
            InternalServerException => (
                environment.IsDevelopment()
                    ? exception.Message
                    : "An error has occurred. Please contact the Administrator.", exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError),
            ValidationException => (exception.Message, exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest),
            BadRequestException => (exception.Message, exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest),
            NotFoundException => (exception.Message, exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound),
            _ => (
                environment.IsDevelopment()
                    ? exception.Message
                    : "An error has occurred. Please contact the Administrator.", exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError)
        };

        var problemDetails = new ProblemDetails
        {
            Title = detail.Title,
            Detail = detail.Detail,
            Status = detail.StatusCode,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

        if (exception is ValidationException validationException)
            problemDetails.Extensions.Add("errors", validationException.Errors);

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}