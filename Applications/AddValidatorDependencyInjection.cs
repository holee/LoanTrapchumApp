namespace Applications;

/// <summary>
/// Validator registration extension.
/// All validators are auto-discovered via <c>AddValidatorsFromAssembly</c>
/// in <see cref="ApplicationServiceInjection"/>.
/// This class is retained for backward compatibility if called externally.
/// </summary>
public static class AddValidatorDependencyInjection
{
    public static IServiceCollection AddValidationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(AddValidatorDependencyInjection).Assembly);
        return services;
    }
}