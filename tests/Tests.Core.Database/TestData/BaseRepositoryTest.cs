// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace Tests.Core.Database.TestData;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseRepositoryTest<TRepository>(InfiniLoreDbContext context, IUnitOfWork<InfiniLoreDbContext> unitOfWork) where TRepository : class, IUnitOfWorkRepository {
    [Before(Test)]
    public async Task TestSetup() {
        await context.Database.EnsureCreatedAsync();
        await context.Database.BeginTransactionAsync();
    }
    
    [After(Test)]
    public async Task TestTeardown() {
        await context.Database.RollbackTransactionAsync();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected async ValueTask<TRepository> GetRepositoryAsync() => await unitOfWork.GetRepositoryAsync<TRepository>();
    
    protected async Task AddModelToDbAsync<TModel>(TModel model) where TModel : class {
        
        DbSet<TModel> dbSet = context.Set<TModel>();
        await dbSet.AddAsync(model);
        await context.SaveChangesAsync();
    }
}
