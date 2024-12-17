// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using InfiniLore.Server.Contracts.Database.Seeding;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InfiniLore.Database.Seeding;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// Seeder should be created once and then never again really?
[InjectableService<ISeederService>(ServiceLifetime.Singleton)]
public class SeederService(IServiceProvider provider) : ISeederService {
    private readonly AsyncServiceScope _scope = provider.CreateAsyncScope();
    private IServiceProvider? _provider;
    private IServiceProvider Provider => _provider ??= _scope.ServiceProvider;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task StartAsync(CancellationToken cancellationToken) {
        // Collect all the types in the current assembly that seed the database
        Assembly currentAssembly = typeof(SeederService).Assembly;

        Task[] seederTasks = currentAssembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(ISeeder)))
            .Select(type => (ISeeder)Provider.GetRequiredService(type))
            .Select(seeder => seeder.StartSeedingAsync(cancellationToken))
            .ToArray();
        
        await Task.WhenAll(seederTasks);
    }
    
    public async Task StopAsync(CancellationToken cancellationToken) {
        await _scope.DisposeAsync();
    }
}
