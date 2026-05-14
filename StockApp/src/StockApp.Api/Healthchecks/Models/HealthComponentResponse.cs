namespace StockApp.Api.Healthchecks.Models;

public class HealthComponentResponse
{
    public string Status { get; set; } = string.Empty;

    public TimeSpan Duration { get; set; }

    public string? Error { get; set; }
}
