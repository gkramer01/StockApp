using FluentValidation;
using MediatR;
using Npgsql;
using Scalar.AspNetCore;
using Serilog;
using StockApp.Api.Controllers;
using StockApp.Api.Middlewares;
using StockApp.Application.Abstractions;
using StockApp.Application.Common.Behaviors;
using StockApp.Application.Features.Products.Create;
using StockApp.Infrastructure.Repositories;
using System.Data;

var builder = WebApplication.CreateBuilder(args);


// --------------------
// Logging (Serilog)
// --------------------
builder.Host.UseSerilog((ctx, lc) =>
    lc.WriteTo.Console());


// --------------------
// Database
// --------------------
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string not found");

builder.Services.AddScoped<IDbConnection>(_ =>
    new NpgsqlConnection(connectionString));


// --------------------
// Dependency Injection
// --------------------
builder.Services.AddScoped<IProductRepository, ProductRepository>();


// --------------------
// MediatR
// --------------------
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateProductHandler).Assembly));


// --------------------
// FluentValidation
// --------------------
builder.Services.AddValidatorsFromAssembly(typeof(CreateProductValidator).Assembly);


// --------------------
// Pipeline Behaviors
// --------------------
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


// --------------------
// OpenAPI + Scalar
// --------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();


var app = builder.Build();


// --------------------
// Middlewares
// --------------------
app.UseGlobalException();


// --------------------
// OpenAPI + Scalar
// --------------------
app.MapOpenApi();
app.MapScalarApiReference();


// --------------------
// Redirect root → Scalar
// --------------------
app.MapGet("/", () => Results.Redirect("/scalar"));


// --------------------
// Endpoints
// --------------------
app.MapProducts();


app.Run();