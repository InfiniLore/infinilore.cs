// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using DataSources.InfiniLore.Server;
using InfiniLore.Server.Modules.Core.Database;
using System.Collections.Frozen;

namespace Tests.InfiniLore.Modules.Core.Database.AccessProtection;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AccessProtectionModelTests {
    [ClassDataSource<ServiceProviderDataSource>(Shared = SharedType.PerTestSession)]
    public required ServiceProviderDataSource ServiceProvider { get; init; }

    private GuidStore GuidStore => ServiceProvider.GetRequiredService<GuidStore>();
    // -----------------------------------------------------------------------------------------------------------------
    // Test Methods
    // -----------------------------------------------------------------------------------------------------------------
    [Test]
    [Arguments(0, 0, true)]
    [Arguments(0, 1, false)]
    public async Task HasPermission_ModelOwner(int ownerSeed, int accessingUserSeed, bool expectedResult) {
        // Arrange
        var model = new AccessProtectionModel {
            ModelOwnerId = GuidStore.GetGuid(ownerSeed)
        };

        // Act
        bool result = model.HasPermission(GuidStore.GetGuid(accessingUserSeed), string.Empty);

        // Assert
        await Assert.That(result).IsEqualTo(expectedResult);
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments(" ")]
    public async Task HasPermission_InvalidPermissions(string? input) {
        // Arrange
        var model = new AccessProtectionModel {
            ModelOwnerId = Guid.NewGuid()
        };

        // Act
        bool result = model.HasPermission(Guid.NewGuid(), input);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments(0, 0, "owner_always_has_access", true)]
    [Arguments(0, 1, "no_permission", false)]
    [Arguments(0, 1, "read", true)]
    [Arguments(0, 1, "write", true)]
    [Arguments(0, 2, "read", false)]
    [Arguments(0, 2, "write", false)]
    public async Task HasPermission_WithExistingPermissions(int ownerSeed, int userSeed, string permission, bool expectedResult) {
        // Arrange
        Guid ownerId = GuidStore.GetGuid(ownerSeed);
        var model = new AccessProtectionModel {
            ModelOwnerId = ownerId
        };
        await Assert.That(model.TryGrantPermission(GuidStore.GetGuid(1), "read")).IsTrue();
        await Assert.That(model.TryGrantPermission(GuidStore.GetGuid(1), "write")).IsTrue();

        // Act
        Guid userId = GuidStore.GetGuid(userSeed);
        bool result = model.HasPermission(userId, permission);

        // Assert
        await Assert.That(result).IsEqualTo(expectedResult);
    }
    
    
    [Test]
    [Arguments(0, "read", true)]
    [Arguments(0, "write", false)] // Already set to write, so should fail
    public async Task TryGrantPermission_DuplicatePermission(int userSeed, string permission, bool expectedResult) {
        // Arrange
        var model = new AccessProtectionModel();
        Guid userId = GuidStore.GetGuid(userSeed);
        model.TryGrantPermission(userId, "write");

        // Act
        bool result = model.TryGrantPermission(userId, permission);

        // Assert
        await Assert.That(result).IsEqualTo(expectedResult);
    }


    [Test]
    [Arguments(null, false)]
    [Arguments("", false)]
    [Arguments(" ", false)]
    [Arguments("read", true)]
    public async Task TryGrantPermission_ValidatesPermissionString(string? permission, bool expectedResult) {
        // Arrange
        var model = new AccessProtectionModel();
        Guid userId = GuidStore.GetGuid(0);

        // Act
        bool result = model.TryGrantPermission(userId, permission!);

        // Assert
        await Assert.That(result).IsEqualTo(expectedResult);
    }

    [Test]
    public async Task TryGrantPermission_ExceedsMaxLength_ReturnsFalse() {
        // Arrange
        var model = new AccessProtectionModel();
        Guid userId = GuidStore.GetGuid(0);
        string longPermission = new('a', AccessProtectionRuleModel.Defaults.PermissionMaxLength + 1);

        // Act
        bool result = model.TryGrantPermission(userId, longPermission);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments(0, "read", true)]
    [Arguments(0, "nonexistent", false)]
    public async Task TryRevokePermission_ExistingPermission(int userSeed, string permission, bool expectedResult) {
        // Arrange
        var model = new AccessProtectionModel();
        Guid userId = GuidStore.GetGuid(userSeed);
        model.TryGrantPermission(userId, "read");

        // Act
        bool result = model.TryRevokePermission(userId, permission);

        // Assert
        await Assert.That(result).IsEqualTo(expectedResult);
    }

    [Test]
    public async Task TryRevokePermission_FromModelOwner_ReturnsFalse() {
        // Arrange
        var model = new AccessProtectionModel {
            ModelOwnerId = GuidStore.GetGuid(0)
        };

        // Act
        bool result = model.TryRevokePermission(model.ModelOwnerId, "read");

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task UserMappedRules_MultiplePermissionsPerUser() {
        // Arrange
        var model = new AccessProtectionModel();
        Guid userId = GuidStore.GetGuid(0);
        
        // Act
        model.TryGrantPermission(userId, "read");
        model.TryGrantPermission(userId, "write");
        
        // Assert
        FrozenDictionary<Guid, AccessProtectionRuleModel[]> mappedRules = model.UserMappedRules;
        await Assert.That(mappedRules[userId].Length).IsEqualTo(2);
        await Assert.That(mappedRules[userId]).Contains(r => r.Permission == "read");
        await Assert.That(mappedRules[userId]).Contains(r => r.Permission == "write");
    }
}
