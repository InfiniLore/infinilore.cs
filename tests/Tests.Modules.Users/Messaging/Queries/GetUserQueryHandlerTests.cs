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
    
    private readonly Guid KnownId = Guid.NewGuid();
    private const string KnownUserName = "AnnaSasDev";
    
    // -----------------------------------------------------------------------------------------------------------------
    // Test Setup
    // -----------------------------------------------------------------------------------------------------------------
    [Before(Test)]
    public async Task TestSetup() {
        await context.Database.EnsureCreatedAsync();

        try {
            await context.Database.BeginTransactionAsync();
        }
        catch {
            // ignored
        }
    }

    [After(Test)]
    public async Task TestTeardown() {
        try {
            await context.Database.RollbackTransactionAsync();
        }
        catch {
            // ignored
        }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Tests
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<Func<GetUserQuery>> TestData() {
        yield return () => GetUserQuery.FromUserId(KnownId);
        yield return () => GetUserQuery.FromUsername(KnownUserName);
    }
    
    [Test]
    [InstanceMethodDataSource(nameof(TestData))]
    public async Task ExecuteAsync_ShouldWork(GetUserQuery query) {
        // Arrange
        var uow = provider.GetRequiredService<IUnitOfWork<InfiniLoreDb>>();
        var repo = await uow.GetRepositoryAsync<UserRepository>();

        var knownUser = new UserModel {
            Id = query.UserId != Guid.Empty ? query.UserId : KnownId,
            UserName = KnownUserName
        };
        RepoOutcome repoOutcome = await repo.AddAsync(knownUser);
        await Assert.That(repoOutcome.IsSuccess).IsTrue();
        await uow.SaveChangesAsync();
        
        GetUserQueryHandler handler = GetHandler();
        
        // Act
        Outcome<UserModel> result = await handler.ExecuteAsync(query, CancellationToken.None);

        // Assert
        await Assert.That(result.TryGetAsSuccess(out UserModel? userModel)).IsTrue();
        await Assert.That(userModel).IsNotNull()
            .And.HasProperty(model => model.Id).IsEqualTo(KnownId)
            .HasProperty(model => model.UserName).IsEqualTo(KnownUserName);
    }
}
