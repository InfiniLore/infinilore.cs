// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Data.System;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Database.Repositories.Content.Data.System;
using JetBrains.Annotations;
using Tests.InfiniLore.Database.Repositories.TestInfrastructure;
using Tests.InfiniLore.Database.Repositories.TestInfrastructure.Repository;

namespace Tests.InfiniLore.Database.Repositories.Content.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(PermissionsRepository))]
[NotInParallel]
[ClassDataSource<DatabaseInfrastructure>(Shared = SharedType.PerTestSession)]
public class BaseContentTests(DatabaseInfrastructure infrastructure) : BaseContentRepositoryTestFramework<PermissionsRepository, InfinilorePermission>(infrastructure) {
    // -----------------------------------------------------------------------------------------------------------------
    // Seeding
    // -----------------------------------------------------------------------------------------------------------------
    public const string PermissionName = "something.else";
    public const string PermissionDescription = "A permission to do Something Else";
    public const string PermissionId = "6f77e414-e716-469e-9c1d-643ae55cd270";
    
    [Before(Test)]
    public async Task SeedDatabase() {
        await CreateSavepointAsync();
        
        // Arrange seed data
        var permission = new InfinilorePermission {
            Id = Guid.Parse(PermissionId),
            Name = PermissionName,
            Description = PermissionDescription
        };
        
        MsSqlDbContext dbContext = await UnitOfWork.GetDbContextAsync();
        
        // Seed database 
        await dbContext.Permissions.AddAsync(permission);

        // Commit changes
        await dbContext.SaveChangesAsync();
    }

    [After(Test)]
    public async Task RunAfterTest() {
        await RollbackToSavepointAsync();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Test Methods
    // -----------------------------------------------------------------------------------------------------------------
    [Test]
    [Arguments("6f77e414-e716-469e-9c1d-643ae55c1234", "something.different", "Something Different")]
    [Arguments("6f77e414-e716-469e-9c1d-643ae55c1234", "something.different", PermissionDescription)] // Description should not be unique and thus using the same description should be fine.
    public async Task TryAddAsync_ShouldReturnSuccess(string? guidValue, string name, string description) {
        // Arrange
        Guid guid = guidValue is not null ? Guid.Parse(guidValue) : Guid.CreateVersion7();
        InfinilorePermission permission = new() {
            Id = guid,
            Name = name,
            Description = description
        };
        
        // Act & Assert
        InfinilorePermission permissionFromDb = await Base_TryAddAsync_ShouldReturnSuccess(permission);
        await Assert.That(permissionFromDb.Name).IsEqualTo(name);
        await Assert.That(permissionFromDb.Description).IsEqualTo(description);
    }
    
    [Test]
    [Arguments(PermissionId, "something.different", "Something Different")]
    [Arguments(null, PermissionName, PermissionDescription)] // name should be unique
    public async Task TryAddAsync_ShouldFail(string? guidValue, string name, string description) {
        // Arrange
        Guid guid = guidValue is not null ? Guid.Parse(guidValue) : Guid.CreateVersion7();
        InfinilorePermission permission = new() {
            Id = guid,
            Name = name,
            Description = description
        };
        
        // Act & Assert
        await Base_TryAddAsync_ShouldFail(permission);
    }
}
