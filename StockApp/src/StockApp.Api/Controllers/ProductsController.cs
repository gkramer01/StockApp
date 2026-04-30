using MediatR;
using StockApp.Application.Features.Products.Create;
using StockApp.Application.Features.Products.GetAll;
using StockApp.Application.Features.Products.GetById;

namespace StockApp.Api.Controllers;

public static class ProductsController
{
    public static void MapProducts(this IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductCommand cmd, IMediator mediator) =>
        {
            var result = await mediator.Send(cmd);

            return result.Success ? Results.Created($"/products/{result.Data}", result) : Results.BadRequest(result.Errors);
        });

        app.MapGet("/products", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllProductsQuery());

            return result.Success
                ? Results.Ok(result.Data)
                : Results.BadRequest(result.Errors);
        });

        app.MapGet("/products/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetProductByIdQuery(id));

            if (!result.Success)
            {
                var error = result.Errors.First();

                if (error.Code == "NOT_FOUND")
                    return Results.NotFound(error.Message);

                return Results.BadRequest(result.Errors);
            }

            return Results.Ok(result.Data);
        });
    }
}