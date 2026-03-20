using DemoPerCorsoClaude.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoPerCorsoClaude.Infrastructure.Data;

/// <summary>DbContext principale dell'applicazione.</summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired();
            entity.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired();
            entity.Property(p => p.Price).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Elettronica", Description = "Dispositivi e accessori elettronici" },
            new Category { Id = 2, Name = "Arredamento", Description = "Mobili e complementi d'arredo" },
            new Category { Id = 3, Name = "Audio", Description = "Cuffie, speaker e accessori audio" }
        );

        modelBuilder.Entity<Product>().HasData(
            new { Id = 1, Name = "Laptop Pro 15", Description = "Laptop professionale 15 pollici", Price = 99.99m, CategoryId = 1, Stock = 25 },
            new { Id = 2, Name = "Mouse Wireless", Description = "Mouse ergonomico senza fili", Price = 29.99m, CategoryId = 1, Stock = 150 },
            new { Id = 3, Name = "Tastiera Meccanica", Description = "Tastiera meccanica RGB", Price = 89.90m, CategoryId = 1, Stock = 75 },
            new { Id = 4, Name = "Scrivania Regolabile", Description = "Scrivania sit-stand motorizzata", Price = 99.00m, CategoryId = 2, Stock = 30 },
            new { Id = 5, Name = "Sedia Ergonomica", Description = "Sedia da ufficio con supporto lombare", Price = 99.00m, CategoryId = 2, Stock = 20 },
            new { Id = 6, Name = "Monitor 27 4K", Description = "Monitor IPS 27 pollici 4K", Price = 99.99m, CategoryId = 1, Stock = 40 },
            new { Id = 7, Name = "Cuffie NC", Description = "Cuffie bluetooth con cancellazione rumore", Price = 79.90m, CategoryId = 3, Stock = 60 },
            new { Id = 8, Name = "Webcam HD", Description = "Webcam 1080p con microfono integrato", Price = 59.99m, CategoryId = 1, Stock = 100 }
        );
    }
}
