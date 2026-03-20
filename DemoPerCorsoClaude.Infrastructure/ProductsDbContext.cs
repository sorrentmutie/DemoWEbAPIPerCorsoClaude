using DemoPerCorsoClaude.Domain;
using Microsoft.EntityFrameworkCore;

namespace DemoPerCorsoClaude.Infrastructure;

/// <summary>EF Core DbContext for the products catalog.</summary>
public class ProductsDbContext(DbContextOptions<ProductsDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
