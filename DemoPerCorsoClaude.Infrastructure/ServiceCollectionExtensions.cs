using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DemoPerCorsoClaude.Infrastructure;

/// <summary>Registration of infrastructure services into the DI container.</summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ProductsDbContext>(opt =>
            opt.UseInMemoryDatabase("ProductsDb"));

        return services;
    }
}
