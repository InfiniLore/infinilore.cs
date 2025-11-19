// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace Tests.Core.Database.TestData;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseRepositoryTest<TRepository>(InfiniLoreDbContext context, IUnitOfWork unitOfWork) where TRepository : class, IUnitOfWorkRepository {
    protected InfiniLoreDbContext Context => context;
    protected IUnitOfWork UnitOfWork => unitOfWork;
    
    [Before(Test)]
    public async Task TestSetup() => await context.Database.EnsureCreatedAsync();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected async ValueTask<TRepository> GetRepositoryAsync() => await UnitOfWork.GetRepositoryAsync<TRepository>();
    
    protected async Task AddModelToDbAsync<TModel>(TModel model) where TModel : class {
        DbSet<TModel> dbSet = Context.Set<TModel>();
        await dbSet.AddAsync(model);
        await Context.SaveChangesAsync();
    }
}
