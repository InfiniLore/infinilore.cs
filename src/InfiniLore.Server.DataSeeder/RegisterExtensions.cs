// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types;
using InfiniLore.Server.DataSeeder.Options;
using InfiniLore.Server.DataSeeder.Seeders;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.DataSeeder;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class WebApplicationBuilderExtensions {
    public static void RegisterDataSeedingServices(this WebApplicationBuilder builder) {
        builder.Services.Configure<SeedingConfig>(builder.Configuration.GetSection("Seeding"));

        // Register all the seeders to the DI container
        builder.Services.RegisterServicesFromInfiniLoreServerDataSeeder();

        builder.Services.AddDataSeederService<OneTimeDataSeederService>(seeder => {
            // One SeederGroup has their seeders run in concurrency
            //      This means they can execute data in "parallel" and therefor can't rely on each-other's data 
            // seeder.AddSeederGroup(group => group.) );

            // User generation depends on a lot of things, and should thus come after "dependency-less" seeders
            seeder.AddSeeder<UserSeeder>();

            // To ensure we don't forget one
            seeder.AddRemainderSeedersAsOneGroup(typeof(IAssemblyEntry).Assembly);
        });
    }
}
