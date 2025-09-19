using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace Infrastructures.DependencyInjects
{

    public static class DatabaseSeeder
    {
        public static async Task SeedSystemData(this IHost builder)
        {
            using var scope = builder.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetService<Seeds.DatabaseSeeder>();
            await seeder!.DatabaseSeederAsync();
        }
    }
}
