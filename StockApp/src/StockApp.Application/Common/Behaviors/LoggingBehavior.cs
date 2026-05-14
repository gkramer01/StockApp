using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace StockApp.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger,
    IHttpContextAccessor httpContextAccessor)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;

    private readonly IHttpContextAccessor _httpContextAccessor =
        httpContextAccessor;

    private static readonly Action<ILogger, string, string?, Exception?>
        _handlingRequest =
            LoggerMessage.Define<string, string?>(
                LogLevel.Information,
                new EventId(1, nameof(Handling)),
                "Handling request {RequestName} with correlation id {CorrelationId}");

    private static readonly Action<ILogger, string, long, string?, Exception?>
        _handledRequest =
            LoggerMessage.Define<string, long, string?>(
                LogLevel.Information,
                new EventId(2, nameof(Handled)),
                "Handled request {RequestName} in {ElapsedMilliseconds}ms with correlation id {CorrelationId}");

    private static readonly Action<ILogger, string, long, string?, Exception?>
        _failedRequest =
            LoggerMessage.Define<string, long, string?>(
                LogLevel.Error,
                new EventId(3, nameof(Failed)),
                "Request {RequestName} failed after {ElapsedMilliseconds}ms with correlation id {CorrelationId}");

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var correlationId =
            _httpContextAccessor.HttpContext?.TraceIdentifier;

        var requestName = typeof(TRequest).Name;

        _handlingRequest(
            _logger,
            requestName,
            correlationId,
            null);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var response = await next(cancellationToken);

            stopwatch.Stop();

            _handledRequest(
                _logger,
                requestName,
                stopwatch.ElapsedMilliseconds,
                correlationId,
                null);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _failedRequest(
                _logger,
                requestName,
                stopwatch.ElapsedMilliseconds,
                correlationId,
                ex);

            throw;
        }
    }

    private static class Handling;
    private static class Handled;
    private static class Failed;
}