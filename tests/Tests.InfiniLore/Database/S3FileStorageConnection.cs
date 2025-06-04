// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using DataSources.InfiniLore.Server;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Shared;

namespace Tests.InfiniLore.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[ClassDataSource<ServiceProviderDataSource>(Shared = SharedType.PerTestSession)]
public class S3FileStorageConnection(ServiceProviderDataSource serviceProvider) {
    private IS3FileStorage S3FileStorageService => serviceProvider.GetRequiredService<IS3FileStorage>();

    // -----------------------------------------------------------------------------------------------------------------
    // Test Methods
    // -----------------------------------------------------------------------------------------------------------------
    [Test]
    public async Task CanConnect() {
        // Arrange
        
        // Act
        Outcome result = await S3FileStorageService.CanConnectAsync();

        // Assert
        await Assert.That(result.TryGetAsState(out bool isConnected)).IsTrue();
        await Assert.That(isConnected).IsTrue();
    }
}
