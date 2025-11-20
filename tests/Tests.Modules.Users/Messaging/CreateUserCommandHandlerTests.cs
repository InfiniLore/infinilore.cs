// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using InfiniLore.Core.Outcomes;
using InfiniLore.Modules.Users.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Modules.Users.Messaging;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[DiDataSource]
public class CreateUserCommandHandlerTests(IServiceProvider provider, InfiniLoreDb context) {
    private CreateUserCommandHandler GetHandler() => ActivatorUtilities.CreateInstance<CreateUserCommandHandler>(provider);

    // -----------------------------------------------------------------------------------------------------------------
    // Test Setup
    // -----------------------------------------------------------------------------------------------------------------
    [Before(Test)]
    public async Task TestSetup() {
        await context.Database.EnsureCreatedAsync();
    }
    
    [After(Test)]
    public Task TestTeardown() 
        => Task.CompletedTask;

    // -----------------------------------------------------------------------------------------------------------------
    // Tests
    // -----------------------------------------------------------------------------------------------------------------
    [Test]
    public async Task ExecuteAsync_ShouldWork() {
        // Arrange
        const string userName = "AnnaSasDev";
        CreateUserCommandHandler handler = GetHandler();
        var command = new CreateUserCommand(userName);
        
        // Act
        Outcome<Guid> result = await handler.ExecuteAsync(command, CancellationToken.None);

        // Assert
        await Assert.That(result.TryGetAsSuccess(out Guid guid)).IsTrue();
        await Assert.That(guid)
            .IsNotDefault()
            .And.IsNotEmptyGuid();
        
        // TODO: implement when we have full database setup
        // await context.Database.CommitTransactionAsync();
        // context.ChangeTracker.Clear();  
        //
        // DbSet<UserModel> userModels = context.Set<UserModel>();
        // bool dbCheckResult = await userModels.AnyAsync(u => u.Id == guid && u.UserName == userName);
        // await Assert.That(dbCheckResult).IsTrue();
    }
}
