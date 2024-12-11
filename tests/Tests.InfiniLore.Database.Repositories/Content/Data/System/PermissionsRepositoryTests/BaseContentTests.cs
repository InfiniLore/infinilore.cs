// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Bogus;
using InfiniLore.Database.Models.Content.Data.System;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Database.Repositories.Content.Data.System;
using JetBrains.Annotations;
using Tests.InfiniLore.Database.Repositories.TestInfrastructure;
using Tests.InfiniLore.Database.Repositories.TestInfrastructure.Repository;

namespace Tests.InfiniLore.Database.Repositories.Content.Data.System.PermissionsRepositoryTests;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(PermissionsRepository))]
[NotInParallel]
[ClassDataSource<DatabaseInfrastructure>(Shared = SharedType.PerTestSession)]
// ReSharper disable once InconsistentNaming
public class Permissions_BaseContentTests(DatabaseInfrastructure infrastructure) : BaseContentRepositoryTestFramework<PermissionsRepository, InfinilorePermission>(infrastructure) {
    // -----------------------------------------------------------------------------------------------------------------
    // Seeding
    // -----------------------------------------------------------------------------------------------------------------
    private const int MaxNameLength = 255;
    private const int MaxDescriptionLength = 511;

    public const string SomethingName = "something.else";
    public const string SomethingDescription = "A permission to do Something Else";
    public const string SomethingId = "6f77e414-e716-469e-9c1d-643ae55cd270";

    public const string SomethingDifferentName = "something.different";
    public const string SomethingDifferentDescription = "A permission to do Something Different";
    public const string SomethingDifferentId = "6f77e414-e716-469e-9c1d-643ae55c1234";

    public const string EmptyId = "00000000-0000-0000-0000-000000000000";

    private static string Truncate(string value, int maxLength) =>
        value.Length > maxLength ? value[..maxLength] : value;

    [Before(Test)]
    public async Task SeedDatabase() {
        await CreateSavepointAsync();

        // Arrange seed data
        var permission1 = new InfinilorePermission {
            Id = Guid.Parse(SomethingId),
            Name = SomethingName,
            Description = SomethingDescription
        };

        var permission2 = new InfinilorePermission {
            Id = Guid.Parse(SomethingDifferentId),
            Name = SomethingDifferentName,
            Description = SomethingDifferentDescription
        };

        // Seed database 
        MsSqlDbContext dbContext = await UnitOfWork.GetDbContextAsync();
        await dbContext.Permissions.AddRangeAsync(permission1, permission2);
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
    [Arguments("79b081c0-fc2d-472f-a38c-68f9caaad863", "something.different.else", "Something Different")]
    [Arguments("e37cd96c-8054-4c68-8dd2-df06985467ea", "something.different.else", SomethingDescription)]// Description should not be unique and thus using the same description should be fine.
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

        await AssertModelExists(permissionFromDb);
    }

    [Test]
    [Arguments(SomethingId, "something.different", "Something Different")]
    [Arguments(null, SomethingName, SomethingDescription)]// name should be unique
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

    [Test]
    [Arguments("c59cbcd6-a28a-4452-be62-02dfa328775e", "something.different.else", "Something Different")]
    [Arguments("79b081c0-fc2d-472f-a38c-68f9caaad863", "something.different.else", SomethingDescription)]// Description should not be unique and thus using the same description should be fine.
    public async Task TryAddAsyncWithResult_ShouldReturnSuccess(string? guidValue, string name, string description) {
        // Arrange
        Guid guid = guidValue is not null ? Guid.Parse(guidValue) : Guid.CreateVersion7();
        InfinilorePermission permission = new() {
            Id = guid,
            Name = name,
            Description = description
        };

        // Act & Assert
        InfinilorePermission permissionFromDb = await Base_TryAddWithResultAsync_ShouldReturnSuccess(permission);
        await Assert.That(permissionFromDb.Name).IsEqualTo(name);
        await Assert.That(permissionFromDb.Description).IsEqualTo(description);

        await AssertModelExists(permissionFromDb);
    }

    [Test]
    [Arguments(SomethingId, "something.different", "Something Different")]
    [Arguments(null, SomethingName, SomethingDescription)]// name should be unique
    public async Task TryAddAsyncWithResult_ShouldFail(string? guidValue, string name, string description) {
        // Arrange
        Guid guid = guidValue is not null ? Guid.Parse(guidValue) : Guid.CreateVersion7();
        InfinilorePermission permission = new() {
            Id = guid,
            Name = name,
            Description = description
        };

        // Act & Assert
        await Base_TryAddWithResultAsync_ShouldFail(permission);
    }

    [Test]
    [Arguments(SomethingId, "updated.name", "Updated Description")]
    public async Task TryUpdateAsync_ShouldReturnSuccess(string guidValue, string name, string description) {
        // Arrange
        Guid guid = Guid.Parse(guidValue);
        InfinilorePermission permission = new() {
            Id = guid,
            Name = name,
            Description = description
        };

        // Act & Assert
        await Base_TryUpdateAsync_ShouldReturnSuccess(permission);
        InfinilorePermission modelFromDb = await AssertModelExists(permission);
        await Assert.That(modelFromDb.Name).IsEqualTo(name);
        await Assert.That(modelFromDb.Description).IsEqualTo(description);
    }

    [Test]
    [Arguments(EmptyId, "updated.name", "Updated Description")]
    public async Task TryUpdateAsync_ShouldFail(string guidValue, string name, string description) {
        // Arrange
        Guid guid = Guid.Parse(guidValue);
        InfinilorePermission permission = new() {
            Id = guid,
            Name = name,
            Description = description
        };

        MsSqlDbContext dbContext = await UnitOfWork.GetDbContextAsync();

        // Act & Assert
        await Base_TryUpdateAsync_ShouldFail(permission);
        dbContext.ChangeTracker.Clear();
        InfinilorePermission? foundModel = dbContext.Permissions.FirstOrDefault(x => x.Id == guid);

        await Assert.That(foundModel).IsNull();
    }

    [Test]
    [Arguments(SomethingId, "updated.name", "Updated Description")]
    public async Task TryUpdateWithResultAsync_ShouldReturnSuccess(string guidValue, string name, string description) {
        // Arrange
        Guid guid = Guid.Parse(guidValue);
        InfinilorePermission permission = new() {
            Id = guid,
            Name = name,
            Description = description
        };

        // Act & Assert
        InfinilorePermission resultModel = await Base_TryUpdateWithResultAsync_ShouldReturnSuccess(permission);
        InfinilorePermission modelFromDb = await AssertModelExists(resultModel);
        await Assert.That(modelFromDb.Name).IsEqualTo(name);
        await Assert.That(modelFromDb.Description).IsEqualTo(description);
    }

    [Test]
    [Arguments(EmptyId, "updated.name", "Updated Description")]
    public async Task TryUpdateWithResultAsync_ShouldFail(string guidValue, string name, string description) {
        // Arrange
        Guid guid = Guid.Parse(guidValue);
        InfinilorePermission permission = new() {
            Id = guid,
            Name = name,
            Description = description
        };

        MsSqlDbContext dbContext = await UnitOfWork.GetDbContextAsync();

        // Act & Assert
        await Base_TryUpdateWithResultAsync_ShouldFail(permission);
        dbContext.ChangeTracker.Clear();
        InfinilorePermission? foundModel = dbContext.Permissions.FirstOrDefault(x => x.Id == guid);

        await Assert.That(foundModel).IsNull();
    }

    [Test]
    public async Task TryUpdateMultiple_ShouldReturnSuccess() {
        // Arrange
        string[] guidValues = [SomethingId, SomethingDifferentId];

        List<InfinilorePermission> permissions = guidValues.Select(guid => new InfinilorePermission {
            Id = Guid.Parse(guid),
            Name = FakerLib.InfinilorePermission.Generate().Name,
            Description = FakerLib.InfinilorePermission.Generate().Description
        }).ToList();

        // Act & Assert
        await Base_TryUpdateRangeAsync_ShouldReturnSuccess(permissions.ToArray());

        foreach (InfinilorePermission permission in permissions) {
            InfinilorePermission modelFromDb1 = await AssertModelExists(permission);
            await Assert.That(modelFromDb1.Name).IsEqualTo(permission.Name);
            await Assert.That(modelFromDb1.Description).IsEqualTo(permission.Description);
        }
    }

    [Test]
    public async Task TryUpdateMultiple_ShouldReturnFailure() {
        // Arrange 
        // - Has one value that doesn't exist on the db yet, and thus should fail.
        string[] guidValues = [SomethingId, SomethingDifferentId, Guid.CreateVersion7().ToString()];
        List<InfinilorePermission> permissions = guidValues.Select(guid => new InfinilorePermission {
            Id = Guid.Parse(guid),
            Name = FakerLib.InfinilorePermission.Generate().Name,
            Description = FakerLib.InfinilorePermission.Generate().Description
        }).ToList();

        // Act & Assert
        await Base_TryUpdateRangeAsync_ShouldReturnFailure(permissions.ToArray());
    }
    
    [Test]
    [Repeat(100)]
    public async Task Repeat_TryAddOrUpdateAsync_ShouldReturnSuccess() {
        // Arrange
        var guid = Guid.NewGuid();
        string name = FakerLib.InfinilorePermission.Generate().Name;
        string description = FakerLib.InfinilorePermission.Generate().Description;
        InfinilorePermission permission = new() {
            Id = guid,
            Name = name,
            Description = description
        };

        // Act & Assert
        InfinilorePermission resultModel = await Base_TryAddOrUpdateAsync_ShouldReturnSuccess(permission);
        InfinilorePermission modelFromDb = await AssertModelExists(resultModel);
        await Assert.That(modelFromDb.Name).IsEqualTo(name);
        await Assert.That(modelFromDb.Description).IsEqualTo(description);
    }
    
    [Test]
    [Arguments("79b081c0-fc2d-472f-a38c-68f9caaad863", "something.different.else", "Something Different")]
    [Arguments(SomethingId, "something.different.that.doesnt.crash", "Something Different")]
    [Arguments(SomethingId, "updated.name", "Updated Description")]
    public async Task TryAddOrUpdateAsync_ShouldReturnSuccess(string guidValue, string name, string description) {
        // Arrange
        Guid guid = Guid.Parse(guidValue);
        InfinilorePermission permission = new() {
            Id = guid,
            Name = name,
            Description = description
        };

        // Act & Assert
        InfinilorePermission resultModel = await Base_TryAddOrUpdateAsync_ShouldReturnSuccess(permission);
        InfinilorePermission modelFromDb = await AssertModelExists(resultModel);
        await Assert.That(modelFromDb.Name).IsEqualTo(name);
        await Assert.That(modelFromDb.Description).IsEqualTo(description);
    }

    [Test]
    public async Task TryAddOrUpdateRangeAsync_ShouldReturnSuccess() {
        // Arrange
        List<InfinilorePermission>? permissions = FakerLib.InfinilorePermission.Generate(100);

        // Act & Assert
        await Base_TryAddOrUpdateRangeAsync_ShouldReturnSuccess(permissions);
        foreach (InfinilorePermission permission in permissions) {
            InfinilorePermission modelFromDb1 = await AssertModelExists(permission);
            await Assert.That(modelFromDb1.Name).IsEqualTo(permission.Name);
            await Assert.That(modelFromDb1.Description).IsEqualTo(permission.Description);
        }
    }

    [Test]
    [Arguments(SomethingId)]
    public async Task TryDeleteAsync_ShouldReturnSuccess(string guidValue) {
        // Arrange
        Guid guid = Guid.Parse(guidValue);
        InfinilorePermission permission = new() {
            Id = guid,
            Name = FakerLib.InfinilorePermission.Generate().Name,
            Description = FakerLib.InfinilorePermission.Generate().Description
        };

        // Act & Assert
        await Base_TryDeleteAsync_ShouldReturnSuccess(permission);
    }

    [Test]
    [Arguments("79b081c0-fc2d-472f-a38c-68f9caaad863")]
    public async Task TryDeleteAsync_ShouldReturnFailure(string guidValue) {
        // Arrange
        Guid guid = Guid.Parse(guidValue);
        InfinilorePermission permission = new() {
            Id = guid,
            Name = FakerLib.InfinilorePermission.Generate().Name,
            Description = FakerLib.InfinilorePermission.Generate().Description
        };

        // Act & Assert
        await Base_TryDeleteAsync_ShouldReturnFailure(permission);
    }

    [Test]
    public async Task TryAddRangeAsync_ShouldReturnSuccess() {
        // Arrange
        List<InfinilorePermission>? permissions = FakerLib.InfinilorePermission.Generate(100);

        // Act & Assert
        await Base_TryAddRange_ShouldReturnSuccess(permissions);
        foreach (InfinilorePermission permission in permissions) {
            InfinilorePermission modelFromDb1 = await AssertModelExists(permission);
            await Assert.That(modelFromDb1.Name).IsEqualTo(permission.Name);
            await Assert.That(modelFromDb1.Description).IsEqualTo(permission.Description);
        }
    }
}
