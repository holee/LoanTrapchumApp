namespace TrapchumApp.Services
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddOtherServices(this IServiceCollection services)
        {

            services.AddSingleton<LoadingTrackerService>();
            services.AddScoped<ExcelImportService>();
            services.AddScoped<ExcelExportService>();
            return services;

        }
    }
}
