// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database.Repositories;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Tests.InfiniLore.Database.Repositories.TestInfrastructure;

namespace Tests.InfiniLore.Database.Repositories.Content.Account.UserRepository;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(global::InfiniLore.Database.Repositories.Content.Account.UserRepository))]
[NotInParallel]
[ClassDataSource<DatabaseInfrastructure>(Shared = SharedType.PerTestSession)]
public class UserHasRolesTests(DatabaseInfrastructure infrastructure) : RepositoryTestFramework<global::InfiniLore.Database.Repositories.Content.Account.UserRepository>(infrastructure) {
    // -----------------------------------------------------------------------------------------------------------------
    // Seeding
    // -----------------------------------------------------------------------------------------------------------------
    [Before(Test)]
    public async Task SeedDatabase() {
        // Arrange seed data
        var originalUser = new InfiniLoreUser { Id = Guid.Parse("bc8caeb2-346e-4754-b05d-8a747a95dc0f"), UserName = "seedTestUser" };
        var roleAdminId = Guid.CreateVersion7();
        var roleEditorId = Guid.CreateVersion7();
        var roleUserId = Guid.CreateVersion7();
        IdentityRole<Guid>[] roles = [
            new() { Id = roleAdminId, Name = "Admin", NormalizedName = "ADMIN" },
            new() { Id = roleEditorId, Name = "Editor", NormalizedName = "EDITOR" },
            new() { Id = roleUserId, Name = "User", NormalizedName = "USER" }
        ];

        MsSqlDbContext dbContext = await UnitOfWork.GetDbContextAsync();

        // Seed database with users and roles
        await dbContext.Users.AddAsync(originalUser);
        await dbContext.Roles.AddRangeAsync(roles);
        await dbContext.UserRoles.AddRangeAsync(
            new IdentityUserRole<Guid> { UserId = originalUser.Id, RoleId = roleAdminId },
            new IdentityUserRole<Guid> { UserId = originalUser.Id, RoleId = roleUserId }
        );

        // Commit changes
        await dbContext.SaveChangesAsync();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Test Methods
    // -----------------------------------------------------------------------------------------------------------------
    [Test]
    [Arguments("bc8caeb2-346e-4754-b05d-8a747a95dc0f", new[] { "Admin", "User" })]
    [Arguments("bc8caeb2-346e-4754-b05d-8a747a95dc0f", new[] { "User" })]
    [Arguments("bc8caeb2-346e-4754-b05d-8a747a95dc0f", new[] { "Admin" })]
    public async Task UserHasAllRoles_ShouldReturnValidUser(string userIdValue, string[] roles) {
        // Arrange
        Guid userId = Guid.Parse(userIdValue);

        // Act
        RepoResult<InfiniLoreUser> result = await Repository.UserHasAllRolesAsync(userId, roles);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.IsFailure).IsFalse();
        await Assert.That(result.AsSuccess.Value.Id).IsEqualTo(userId);
    }

    [Test]
    [Arguments("bc8caeb2-346e-4754-b05d-8a747a95dc0f", new[] { "Admin", "Editor" })]
    [Arguments("bc8caeb2-346e-4754-b05d-8a747a95dc0f", new[] { "Editor" })]
    public async Task UserHasAllRoles_ShouldReturnFailure(string userIdValue, string[] roles) {
        // Arrange
        Guid userId = Guid.Parse(userIdValue);

        // Act
        RepoResult<InfiniLoreUser> result = await Repository.UserHasAllRolesAsync(userId, roles);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.IsSuccess).IsFalse();
        await Assert.That(result.AsFailure.Value).IsEqualTo("User does not have all roles.");
    }

    [Test]
    [Arguments("d9494938-0bef-47b5-9a92-6f10d26779a6", new[] { "Admin", "User" })]
    public async Task UserHasAllRoles_ShouldReturnFailureUserDoesntExist(string userIdValue, string[] roles) {
        // Arrange
        Guid userId = Guid.Parse(userIdValue);

        // Act
        RepoResult<InfiniLoreUser> result = await Repository.UserHasAllRolesAsync(userId, roles);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.IsSuccess).IsFalse();
        await Assert.That(result.AsFailure.Value).IsEqualTo("User not found.");
    }
}
