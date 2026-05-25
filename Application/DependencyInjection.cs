using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registers application-layer services.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);
        });

        RegisterValidators(services, assembly);

        return services;
    }

    private static void RegisterValidators(IServiceCollection services, Assembly assembly)
    {
        foreach (var validatorType in assembly.DefinedTypes)
        {
            if (validatorType is { IsAbstract: true } || validatorType.IsInterface)
            {
                continue;
            }

            foreach (var serviceType in validatorType.ImplementedInterfaces)
            {
                if (!serviceType.IsGenericType || serviceType.GetGenericTypeDefinition() != typeof(IValidator<>))
                {
                    continue;
                }

                services.AddScoped(serviceType, validatorType.AsType());
            }
        }
    }
}
