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
[DiDataSource]
public class S3FileStorageConnection(IS3FileStorageService s3FileStorageService) {
    [Test]
    public async Task CanConnect() {
        // Arrange
        
        // Act
        Result result = await s3FileStorageService.CanConnectAsync();

        // Assert
        await Assert.That(result.TryGetState(out bool isConnected)).IsTrue();
        await Assert.That(isConnected).IsTrue();
    }
}
