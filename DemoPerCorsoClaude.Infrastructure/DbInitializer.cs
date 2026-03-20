using DemoPerCorsoClaude.Domain;

namespace DemoPerCorsoClaude.Infrastructure;

/// <summary>Seeds the in-memory database with initial data.</summary>
public static class DbInitializer
{
    public static void Seed(ProductsDbContext context)
    {
        if (context.Categories.Any())
            return;

        context.Categories.AddRange(
            new Category(1, "Periferiche"),
            new Category(2, "Monitor"),
            new Category(3, "Accessori")
        );
        context.SaveChanges();

        context.Products.AddRange(
            new Product(1, "Tastiera meccanica", "Switch Cherry MX Red, layout IT", 89.99m, 42, 1),
            new Product(2, "Monitor 27\"",        "4K IPS 144Hz, HDR400",           349.00m, 15, 2),
            new Product(3, "Mouse wireless",      "Sensore ottico 25600 DPI",        59.90m, 78, 1),
            new Product(4, "Webcam HD",           "1080p 60fps con microfono",        74.50m, 30, 3),
            new Product(5, "Hub USB-C",           "7 porte, Power Delivery 100W",    44.00m, 60, 3)
        );
        context.SaveChanges();
    }
}
