// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Database.Seeding;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InfiniLore.Database.Seeding;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SeederService(IServiceProvider provider) : ISeederService {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task StartAsync(CancellationToken cancellationToken) {
        // Collect all the types in the current assembly that seed the database
        Assembly currentAssembly = typeof(SeederService).Assembly;
        
        IEnumerable<Type> types = currentAssembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ISeeder)));
        foreach (Type type in types) {
            AsyncServiceScope scope = provider.CreateAsyncScope();
            IServiceProvider scopedProvider = scope.ServiceProvider;
            
            // Every seeder has their own scope
            var seeder = (ISeeder)scopedProvider.GetRequiredService(type);
            await seeder.StartSeedingAsync(cancellationToken);
            
            await scope.DisposeAsync();
        }
    }
    
    public Task StopAsync(CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }
}
