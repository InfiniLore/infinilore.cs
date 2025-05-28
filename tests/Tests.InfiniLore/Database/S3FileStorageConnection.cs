// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using DataSources.InfiniLore.Server;
using InfiniLore.Server.Modules.Core;

namespace Tests.InfiniLore.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class S3FileStorageConnection {
    [ClassDataSource<ServiceProviderDataSource>(Shared = SharedType.PerAssembly)]
    public required ServiceProviderDataSource ServiceProvider { get; init; }
    
    [Test]
    public async Task CanConnect() {
        // Arrange
        var s3FileStorageService = ServiceProvider.GetRequiredService<IS3FileStorageService>();
        
        // Act
        Result result = await s3FileStorageService.CanConnectAsync();

        // Assert
        await Assert.That(result.TryGetState(out bool isConnected)).IsTrue();
        await Assert.That(isConnected).IsTrue();
    }
}
