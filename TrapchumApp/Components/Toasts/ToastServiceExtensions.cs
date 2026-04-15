using Microsoft.Extensions.DependencyInjection;

namespace TrapchumApp.Components.Toasts;

public static class ToastServiceExtensions
{
    /// <summary>
    /// Registers <see cref="ToastService"/> as a scoped service with optional configuration.
    /// </summary>
    public static IServiceCollection AddToastService(this IServiceCollection services, Action<ToastOptions>? configure = null)
    {
        var options = new ToastOptions();

        configure?.Invoke(options);

        services.AddScoped(_ => new ToastService(options));

        return services;
    }
}
