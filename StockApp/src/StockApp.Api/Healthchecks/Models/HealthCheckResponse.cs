namespace StockApp.Api.Healthchecks.Models;

public class HealthCheckResponse
{
    public string Status { get; set; } = string.Empty;

    public Dictionary<string, HealthComponentResponse> Components
    { get; set; } = [];

    public DateTime Timestamp { get; set; }
}