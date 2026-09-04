using Application;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    /// <summary>
    /// Registers all Application layer services, mediator handlers, validators, and policies.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddApplicationServices();
        services.AddValidationServices();
        return services;
    }
}
