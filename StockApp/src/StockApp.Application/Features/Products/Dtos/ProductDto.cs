namespace StockApp.Application.Features.Products.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public int Quantity { get; init; }
    }
}
