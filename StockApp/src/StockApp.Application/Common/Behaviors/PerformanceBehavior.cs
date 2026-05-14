using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace StockApp.Application.Common.Behaviors;

public class PerformanceBehavior<TRequest, TResponse>(
    ILogger<PerformanceBehavior<TRequest, TResponse>> logger,
    IHttpContextAccessor httpContextAccessor)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private const int SlowRequestThresholdMs = 500;

    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>>
        _logger = logger;

    private readonly IHttpContextAccessor _httpContextAccessor =
        httpContextAccessor;

    private static readonly Action<ILogger, string, long, string?, Exception?>
        _slowRequest =
            LoggerMessage.Define<string, long, string?>(
                LogLevel.Warning,
                new EventId(1, nameof(SlowRequest)),
                "Slow request detected: {RequestName} took {ElapsedMilliseconds}ms with correlation id {CorrelationId}");

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        var response = await next(cancellationToken);

        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > SlowRequestThresholdMs)
        {
            var requestName = typeof(TRequest).Name;

            var correlationId =
                _httpContextAccessor.HttpContext?.TraceIdentifier;

            _slowRequest(
                _logger,
                requestName,
                stopwatch.ElapsedMilliseconds,
                correlationId,
                null);
        }

        return response;
    }

    private static class SlowRequest;
}