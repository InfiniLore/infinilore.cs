// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using InfiniLore.Core.Outcomes;
using InfiniLore.Modules.Users.Database;
using InfiniLore.Modules.Users.Messaging.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Modules.Users.Messaging.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[DiDataSource]
public class CreateUserCommandHandlerTests(IServiceProvider provider) {
    private CreateUserCommandHandler GetHandler() => ActivatorUtilities.CreateInstance<CreateUserCommandHandler>(provider);

    // -----------------------------------------------------------------------------------------------------------------
    // Test Setup
    // -----------------------------------------------------------------------------------------------------------------
    [Before(Test)]
    public async Task TestSetup() {
        var contextFactory = provider.GetRequiredService<IDbContextFactory<InfiniLoreDb>>();
        await using InfiniLoreDb context = await contextFactory.CreateDbContextAsync();
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
        
        var contextFactory = provider.GetRequiredService<IDbContextFactory<InfiniLoreDb>>();
        await using InfiniLoreDb context = await contextFactory.CreateDbContextAsync();
        context.ChangeTracker.Clear();  
        
        UserModel? user = await context.Set<UserModel>().FindAsync(guid);
        await Assert.That(user)
            .IsNotNull()
            .And.HasProperty(model => model.UserName).IsEqualTo(userName);
    }
}
