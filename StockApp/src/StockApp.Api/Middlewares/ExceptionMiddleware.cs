using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StockApp.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace StockApp.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Unhandled exception occurred");

            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            ValidationException validationException =>
                CreateValidationProblem(validationException),

            NotFoundException =>
                CreateProblemDetails(
                    HttpStatusCode.NotFound,
                    exception.Message),

            DomainException =>
                CreateProblemDetails(
                    HttpStatusCode.Conflict,
                    exception.Message),

            _ =>
                CreateProblemDetails(
                    HttpStatusCode.InternalServerError,
                    "An unexpected error occurred")
        };

        context.Response.StatusCode = response.Status ?? 500;

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }

    private static ProblemDetails CreateProblemDetails(
        HttpStatusCode statusCode,
        string detail)
    {
        return new ProblemDetails
        {
            Status = (int)statusCode,
            Title = statusCode.ToString(),
            Detail = detail
        };
    }

    private static ValidationProblemDetails CreateValidationProblem(
        ValidationException validationException)
    {
        var errors = validationException.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray());

        return new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed"
        };
    }
}