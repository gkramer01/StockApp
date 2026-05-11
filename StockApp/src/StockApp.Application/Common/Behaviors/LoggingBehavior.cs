using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace StockApp.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;

    private static readonly Action<ILogger, string, Exception?> _handlingRequest =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1, nameof(Handling)),
            "Handling request {RequestName}");

    private static readonly Action<ILogger, string, long, Exception?> _handledRequest =
        LoggerMessage.Define<string, long>(
            LogLevel.Information,
            new EventId(2, nameof(Handled)),
            "Handled request {RequestName} in {ElapsedMilliseconds}ms");

    private static readonly Action<ILogger, string, long, Exception?> _failedRequest =
        LoggerMessage.Define<string, long>(
            LogLevel.Error,
            new EventId(3, nameof(Failed)),
            "Request {RequestName} failed after {ElapsedMilliseconds}ms");

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _handlingRequest(_logger, requestName, null);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var response = await next(cancellationToken);

            stopwatch.Stop();

            _handledRequest(
                _logger,
                requestName,
                stopwatch.ElapsedMilliseconds,
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
                ex);

            throw;
        }
    }

    private static class Handling;
    private static class Handled;
    private static class Failed;
}