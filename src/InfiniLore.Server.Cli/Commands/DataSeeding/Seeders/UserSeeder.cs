// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;

namespace InfiniLore.Server.Cli.DataSeeding.Seeders;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<ISeeder>("user")]
public class UserSeeder : ISeeder {
    public Task StartSeedingAsync(CancellationToken ct = default) => throw new NotImplementedException();
}
