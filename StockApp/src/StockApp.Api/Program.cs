using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StockApp.Api.Configuration;
using StockApp.Api.Controllers;
using StockApp.Api.Middlewares;
using StockApp.Application.Abstractions;
using StockApp.Application.Common.Behaviors;
using StockApp.Application.Features.Products.Create;
using StockApp.Infrastructure.Context;
using StockApp.Infrastructure.Persistence;
using StockApp.Infrastructure.Repositories;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


// --------------------
// Logging (Serilog)
// --------------------
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});


// --------------------
// Database
// --------------------
var connectionString = builder.Configuration
    .GetConnectionString("Default")
    ?? throw new InvalidOperationException(
        "Connection string not found");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new DbConnectionFactory(connectionString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// --------------------
// Health Checks
// --------------------
builder.Services.AddHealthChecksConfiguration(connectionString);


// --------------------
// Repositories
// --------------------
builder.Services.AddScoped<IProductRepository, ProductRepository>();


// --------------------
// MediatR
// --------------------
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(CreateProductHandler).Assembly);
});


// --------------------
// FluentValidation
// --------------------
builder.Services.AddValidatorsFromAssembly(
    typeof(CreateProductValidator).Assembly);


// --------------------
// Pipeline Behaviors
// --------------------
builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(LoggingBehavior<,>));

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(PerformanceBehavior<,>));

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(TransactionBehavior<,>));


// --------------------
// HttpContext
// --------------------
builder.Services.AddHttpContextAccessor();


// --------------------
// Swagger
// --------------------
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


var app = builder.Build();


// --------------------
// Apply migrations
// --------------------
using (var scope = app.Services.CreateScope())
{
    var dbContext =
        scope.ServiceProvider.GetRequiredService<AppDbContext>();

    const int maxRetries = 10;

    for (int retry = 1; retry <= maxRetries; retry++)
    {
        try
        {
            await dbContext.Database.MigrateAsync();
            break;
        }
        catch
        {
            if (retry == maxRetries)
                throw;

            await Task.Delay(2000);
        }
    }
}


// --------------------
// Middlewares
// --------------------
app.UseCorrelationId();

app.UseSerilogRequestLogging();

app.UseGlobalException();


// --------------------
// Swagger
// --------------------
app.UseSwagger();

app.UseSwaggerUI();


// --------------------
// Endpoints
// --------------------
app.MapProducts();

app.UseHealthChecksConfiguration();

app.Run();