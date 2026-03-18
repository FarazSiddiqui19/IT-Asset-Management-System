using IT_Asset_Management_System.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace IT_Asset_Management_System.Middleware
{
   

    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

            var (statusCode, message) = exception switch
            {
                NotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
                ValidationException ex => (StatusCodes.Status400BadRequest, ex.Message),
                ForbiddenException ex => (StatusCodes.Status403Forbidden, ex.Message),
                ConflictException ex => (StatusCodes.Status409Conflict, ex.Message),
                UnauthorizedException ex => (StatusCodes.Status401Unauthorized, ex.Message),
                InternalServerException ex => (StatusCodes.Status500InternalServerError, ex.Message),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")

            };

            context.Response.StatusCode = statusCode;

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = "Validation Error",
                Detail = message,
                Type = exception.GetType().Name
            };

            await context.Response.WriteAsJsonAsync(problem,cancellationToken);

            return true; // true = exception is handled, stop propagation
        }

       
    }
}

