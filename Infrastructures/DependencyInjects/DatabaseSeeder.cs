using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructures.DependencyInjects
{

    public static class DatabaseSeeder
    {
        public static async Task SeedSystemData(this WebApplication builder)
        {
            using var scope = builder.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetService<Seeds.DatabaseSeeder>();
            await seeder!.DatabaseSeederAsync();
        }
    }
}
