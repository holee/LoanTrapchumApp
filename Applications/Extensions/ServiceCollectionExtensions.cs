using System.Reflection;

namespace Applications.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRequestHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var handlerInterfaceType = typeof(IRequestHandler<,>);

        // Track already-registered service types to prevent duplicates
        var registeredServiceTypes = new HashSet<Type>();

        var implementationTypes = assembly
            .GetExportedTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false });

        foreach (var implementationType in implementationTypes)
        {
            var handlerInterfaces = implementationType
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType);

            foreach (var serviceType in handlerInterfaces)
            {
                if (registeredServiceTypes.Add(serviceType))
                {
                    services.AddScoped(serviceType, implementationType);
                }
            }
        }

        return services;
    }
}
