using Application.Abstractions.Persistence;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers infrastructure-layer services.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        services.AddSingleton<IUnitOfWork, InMemoryUnitOfWork>();

        return services;
    }
}
