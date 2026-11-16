using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DevHabit.Api.Middleware;

public sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception, 
        CancellationToken cancellationToken)
    {
        ProblemDetailsContext context = new()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Title = exception.GetType().Name, Detail = "An error occurred while processing the request.",
            }
        };

        return await problemDetailsService.TryWriteAsync(context);
    }
}