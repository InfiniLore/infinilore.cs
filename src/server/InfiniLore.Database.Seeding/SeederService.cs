// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Database.Seeding;
using Microsoft.Extensions.DependencyInjection;

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
        IEnumerable<Task> tasks  = typeof(SeederService).Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(ISeeder)))
            .Select(t => (Type: t, CancellationToken: cancellationToken, Provider: provider))
            // Create the tasks with each their own scope
            .Select(static async tuple => {
                (Type type, CancellationToken ct, IServiceProvider provider) = tuple;
                
                AsyncServiceScope scope = provider.CreateAsyncScope();
                IServiceProvider scopedProvider = scope.ServiceProvider;

                var seeder = (ISeeder)scopedProvider.GetRequiredService(type);
                await seeder.StartSeedingAsync(ct);
            });
        
        await Task.WhenAll(tasks);
    }
    
    public Task StopAsync(CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }
}
