// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RepositoryTests<TRepository, TModel>(IServiceProvider serviceProvider)
    where TModel : BaseModel, new()
    where TRepository : BaseModelRepository<TModel>, IUnitOfWorkRepository {
    protected IUnitOfWork<InfiniLoreDb> UnitOfWork => serviceProvider.GetRequiredService<IUnitOfWork<InfiniLoreDb>>();
    protected InfiniLoreDb InfiniLoreDb => serviceProvider.GetRequiredService<InfiniLoreDb>();

    // -----------------------------------------------------------------------------------------------------------------
    // Test Config
    // -----------------------------------------------------------------------------------------------------------------
    protected async Task DbSetupAsync() {
        await InfiniLoreDb.Database.EnsureCreatedAsync();
        try {
            await InfiniLoreDb.Database.BeginTransactionAsync();
        }
        catch {
            // ignored
        }
    }

    protected async Task DbTeardownAsync() {
        try {
            await InfiniLoreDb.Database.RollbackTransactionAsync();
        }
        catch {
            // ignored
        }
        
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected async ValueTask<TRepository> GetRepositoryAsync()
        => await UnitOfWork.GetRepositoryAsync<TRepository>();

    protected async Task AddModelToDbAsync<T>(T model) where T : class {
        DbSet<T> dbSet = InfiniLoreDb.Set<T>();
        await dbSet.AddAsync(model);
        await InfiniLoreDb.SaveChangesAsync();
    }

    protected async Task<TModel> GetModelFromDbAsync(Guid id) {
        DbSet<TModel> dbSet = InfiniLoreDb.Set<TModel>();
        TModel? model = await dbSet.FindAsync(id);
        return model ?? throw new Exception($"Model with id {id} not found.");
    }
}
