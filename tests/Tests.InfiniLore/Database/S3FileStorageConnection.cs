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
[ClassDataSource<ServiceProviderDataSource>(Shared = SharedType.PerTestSession)]
public class S3FileStorageConnection(ServiceProviderDataSource serviceProvider) {
    private IS3FileStorageService S3FileStorageService => serviceProvider.GetRequiredService<IS3FileStorageService>();

    // -----------------------------------------------------------------------------------------------------------------
    // Test Methods
    // -----------------------------------------------------------------------------------------------------------------
    [Test]
    public async Task CanConnect() {
        // Arrange
        
        // Act
        Result result = await S3FileStorageService.CanConnectAsync();

        // Assert
        await Assert.That(result.TryGetState(out bool isConnected)).IsTrue();
        await Assert.That(isConnected).IsTrue();
    }
}
