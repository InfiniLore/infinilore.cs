// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.InfiniLore.Database.Repositories.TestInfrastructure.Repository;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RepositoryTestFramework(DatabaseInfrastructure infrastructure) {
    private readonly Guid TransactionId = Guid.NewGuid();
    protected readonly IUnitOfWork UnitOfWork = infrastructure.ServiceProvider.GetRequiredService<IUnitOfWorkFactory>().Create();

    protected async Task<ContentDbContext> InitializeDbContextAsync() {
        var dbContext = await UnitOfWork.GetDbContextAsync<ContentDbContext>();
        await UnitOfWork.TryCreateTransactionAsync();
        await UnitOfWork.TryCreateSavepointAsync(TransactionId);
        
        return dbContext;
    }

    protected async Task CleanupDbContextAsync() {
        var result = await UnitOfWork.TryRollbackToSavepointAsync(TransactionId);
        await UnitOfWork.DisposeAsync();
        // await Assert.That(result).IsTrue();
    }
}
