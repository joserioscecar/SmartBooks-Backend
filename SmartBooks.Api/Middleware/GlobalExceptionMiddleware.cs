using Microsoft.AspNetCore.Mvc;
using SmartBooks.Domain.Exceptions;

namespace SmartBooks.Api.Middleware;

public class GlobalExceptionMiddleware
{

    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IProblemDetailsService _problemDetailsService;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IProblemDetailsService problemDetailsService)
    {
        _next = next;
        _logger = logger;
        _problemDetailsService = problemDetailsService;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(httpContext, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        _logger.LogError(exception, "Unhandled exception. TraceId: {TraceId}", httpContext.TraceIdentifier);

        var response = exception switch
        {
            RuleBusinessException ex => new ErrorResponse(
                StatusCodes.Status400BadRequest,
                "Business rule violation",
                "https://httpstatuses.com/400",
                ex.Code
            ),

            KeyNotFoundException => new ErrorResponse(
                StatusCodes.Status404NotFound,
                "Resource not found",
                "https://httpstatuses.com/404",
                "NOT_FOUND"
            ),

            _ => new ErrorResponse(
                StatusCodes.Status500InternalServerError,
                "Internal server error",
                "https://httpstatuses.com/500",
                "INTERNAL_SERVER_ERROR"
            )
        };

        httpContext.Response.StatusCode = response.Status;
        httpContext.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Status = response.Status,
            Title = response.Title,
            Type = response.Type,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["code"] = response.Code;
        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        await _problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
            Exception = exception
        });
    }

    private sealed record ErrorResponse(int Status, string Title, string Type, string Code);
}
