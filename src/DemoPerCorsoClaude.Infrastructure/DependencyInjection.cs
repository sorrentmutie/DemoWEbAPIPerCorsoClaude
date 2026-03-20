using DemoPerCorsoClaude.Domain.Services;
using DemoPerCorsoClaude.Infrastructure.Data;
using DemoPerCorsoClaude.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DemoPerCorsoClaude.Infrastructure;

/// <summary>Registrazione dei servizi di persistenza.</summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("DemoDb"));

        services.AddScoped<IProductRepository, EfProductRepository>();

        return services;
    }
}
