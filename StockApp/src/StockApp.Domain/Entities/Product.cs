namespace StockApp.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public int Quantity { get; private set; }

    public Product(string name, int quantity = 0)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");

        Id = Guid.NewGuid();
        Name = name;
        Quantity =  quantity;
    }
}
