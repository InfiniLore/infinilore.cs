// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace Tests.Core.Database.TestData;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseRepositoryTest<TRepository>(InfiniLoreDb context, IUnitOfWork<InfiniLoreDb> unitOfWork) where TRepository : class, IUnitOfWorkRepository {
    protected IUnitOfWork<InfiniLoreDb> UnitOfWork => unitOfWork;
    
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
    protected async ValueTask<TRepository> GetRepositoryAsync() 
        => await unitOfWork.GetRepositoryAsync<TRepository>();
    
    protected async Task AddModelToDbAsync<TModel>(TModel model) where TModel : class {
        DbSet<TModel> dbSet = context.Set<TModel>();
        await dbSet.AddAsync(model);
        await context.SaveChangesAsync();
    }
    
    protected async Task<TModel> GetModelFromDbAsync<TModel>(Guid id) where TModel : class {
        DbSet<TModel> dbSet = context.Set<TModel>();
        TModel? model = await dbSet.FindAsync(id);
        return model ?? throw new Exception($"Model with id {id} not found.");
    }
}
