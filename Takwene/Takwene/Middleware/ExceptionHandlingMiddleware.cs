using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Takwene.Application.Exceptions;

namespace Takwene.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                var problem = new ValidationProblemDetails(
                    ex.Errors
                      .GroupBy(e => e.PropertyName)
                      .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()))
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "One or more validation errors occurred."
                };
                await WriteAsync(context, problem, HttpStatusCode.BadRequest);
            }
            catch (NotFoundException ex)
            {
                await WriteAsync(context, Problem("Resource not found", ex.Message, HttpStatusCode.NotFound), HttpStatusCode.NotFound);
            }
            catch (ConflictException ex)
            {
                await WriteAsync(context, Problem("Conflict", ex.Message, HttpStatusCode.Conflict), HttpStatusCode.Conflict);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                // Do NOT leak internal details to the client.
                await WriteAsync(context, Problem("Server error", "An unexpected error occurred.", HttpStatusCode.InternalServerError), HttpStatusCode.InternalServerError);
            }
        }

        private static ProblemDetails Problem(string title, string detail, HttpStatusCode status) =>
            new ProblemDetails { Title = title, Detail = detail, Status = (int)status };

        private static async Task WriteAsync(HttpContext context, ProblemDetails problem, HttpStatusCode status)
        {
            context.Response.StatusCode = (int)status;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
