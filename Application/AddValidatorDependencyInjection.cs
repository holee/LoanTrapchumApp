namespace Application;

public static class AddValidatorDependencyInjection
{
    public static IServiceCollection AddValidationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(AddValidatorDependencyInjection).Assembly);

        return services;
    }
}