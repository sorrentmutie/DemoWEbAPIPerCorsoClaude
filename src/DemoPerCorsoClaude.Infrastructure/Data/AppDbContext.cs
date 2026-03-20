using DemoPerCorsoClaude.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoPerCorsoClaude.Infrastructure.Data;

/// <summary>DbContext principale dell'applicazione.</summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired();
            entity.Property(p => p.Category).IsRequired();
            entity.Property(p => p.Price).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop Pro 15", Description = "Laptop professionale 15 pollici", Price = 1299.99m, Category = "Elettronica", Stock = 25 },
            new Product { Id = 2, Name = "Mouse Wireless", Description = "Mouse ergonomico senza fili", Price = 29.99m, Category = "Elettronica", Stock = 150 },
            new Product { Id = 3, Name = "Tastiera Meccanica", Description = "Tastiera meccanica RGB", Price = 89.90m, Category = "Elettronica", Stock = 75 },
            new Product { Id = 4, Name = "Scrivania Regolabile", Description = "Scrivania sit-stand motorizzata", Price = 449.00m, Category = "Arredamento", Stock = 30 },
            new Product { Id = 5, Name = "Sedia Ergonomica", Description = "Sedia da ufficio con supporto lombare", Price = 349.00m, Category = "Arredamento", Stock = 20 },
            new Product { Id = 6, Name = "Monitor 27 4K", Description = "Monitor IPS 27 pollici 4K", Price = 399.99m, Category = "Elettronica", Stock = 40 },
            new Product { Id = 7, Name = "Cuffie NC", Description = "Cuffie bluetooth con cancellazione rumore", Price = 199.90m, Category = "Audio", Stock = 60 },
            new Product { Id = 8, Name = "Webcam HD", Description = "Webcam 1080p con microfono integrato", Price = 59.99m, Category = "Elettronica", Stock = 100 }
        );
    }
}
