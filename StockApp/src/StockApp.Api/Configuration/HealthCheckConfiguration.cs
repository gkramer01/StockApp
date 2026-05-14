using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using StockApp.Api.Healthchecks.Models;

namespace StockApp.Api.Configuration;

public static class HealthCheckConfiguration
{
    public static IServiceCollection AddHealthChecksConfiguration(
        this IServiceCollection services,
        string connectionString)
    {
        services
            .AddHealthChecks()
            .AddNpgSql(
                connectionString,
                name: "postgres");

        return services;
    }

    public static IApplicationBuilder UseHealthChecksConfiguration(
        this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType =
                    "application/json";

                var response = new HealthCheckResponse
                {
                    Status = report.Status.ToString(),
                    Timestamp = DateTime.UtcNow,
                    Components = report.Entries.ToDictionary(
                        entry => entry.Key,
                        entry => new HealthComponentResponse
                        {
                            Status =
                                entry.Value.Status.ToString(),

                            Duration =
                                entry.Value.Duration,

                            Error =
                                entry.Value.Exception?.Message
                        })
                };

                await context.Response.WriteAsJsonAsync(
                    response);
            }
        });

        return app;
    }
}