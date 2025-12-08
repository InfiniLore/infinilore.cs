// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using InfiniLore.Core.Outcomes;
using InfiniLore.Modules.Users.Database;
using InfiniLore.Modules.Users.Messaging.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Modules.Users.Messaging.Queries;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[DiDataSource]
public class GetUserQueryHandlerTests(IServiceProvider provider, InfiniLoreDb context) {
    private GetUserQueryHandler GetHandler() => ActivatorUtilities.CreateInstance<GetUserQueryHandler>(provider);
    
    private const string KnownUserName = "AnnaSasDev";
    
    // -----------------------------------------------------------------------------------------------------------------
    // Test Setup
    // -----------------------------------------------------------------------------------------------------------------
    [Before(Test)]
    public async Task TestSetup() {
        await context.Database.EnsureCreatedAsync();
    }

    [After(Test)]
    public async Task TestTeardown() {
        await Task.CompletedTask;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Tests
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<(GetUserQuery Query, Guid ExpectedId)>> TestData() {
        var userId = Guid.NewGuid();
        yield return () => (GetUserQuery.FromUserId(userId), userId);
        yield return () => (GetUserQuery.FromUsername(KnownUserName), Guid.NewGuid());
    }

    [Test]
    [MethodDataSource(nameof(TestData))]
    public async Task ExecuteAsync_ShouldWork(GetUserQuery query, Guid expectedId) {
        // Arrange
        var uow = provider.GetRequiredService<IUnitOfWork<InfiniLoreDb>>();
        var repo = await uow.GetRepositoryAsync<UserModelRepository>();

        bool result = await repo.AddAsync(new UserModel {
            Id = expectedId,
            UserName = KnownUserName
        });
        
        await Assert.That(result).IsTrue();
        await uow.SaveChangesAsync();
    
        GetUserQueryHandler handler = GetHandler();
    
        // Act
        Outcome<UserModel> outcome = await handler.ExecuteAsync(query, CancellationToken.None);

        // Assert
        await Assert.That(outcome.TryGetAsData(out UserModel? userModel)).IsTrue();
        await Assert.That(userModel).IsNotNull()
            .And.HasProperty(model => model.Id).IsEqualTo(expectedId)
            .HasProperty(model => model.UserName).IsEqualTo(KnownUserName);
    }
}
