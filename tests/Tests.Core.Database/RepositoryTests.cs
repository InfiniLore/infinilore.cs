// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Bogus;
using InfiniLore.Core.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RepositoryTests<TRepository, TModel>(IServiceProvider serviceProvider)
    where TModel : BaseModel, new()
    where TRepository : BaseModelRepository<TModel>, IUnitOfWorkRepository 
{
    
    protected IUnitOfWork<InfiniLoreDb> UnitOfWork => serviceProvider.GetRequiredService<IUnitOfWork<InfiniLoreDb>>();
    private InfiniLoreDb InfiniLoreDb => serviceProvider.GetRequiredService<InfiniLoreDb>();

    // -----------------------------------------------------------------------------------------------------------------
    // Test Config
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Prepares the database for testing by ensuring it is created and attempting to start a transaction.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async Task DbSetupAsync() {
        await InfiniLoreDb.Database.EnsureCreatedAsync();
        try {
            await InfiniLoreDb.Database.BeginTransactionAsync();
        }
        catch {
            // ignored
        }
    }

    /// <summary>
    /// Tears down the database state asynchronously by attempting to roll back any active transactions.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
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
    /// <summary>
    /// Retrieves an instance of the repository asynchronously from the unit of work.
    /// </summary>
    /// <typeparam name="TRepository">The type of the repository to retrieve.</typeparam>
    /// <returns>A task that represents the asynchronous operation. The task result contains the requested repository instance.</returns>
    protected async ValueTask<TRepository> GetRepositoryAsync()
        => await UnitOfWork.GetRepositoryAsync<TRepository>();

    /// <summary>
    /// Configures a provided instance of the Faker class with additional rules for generating TModel instances.
    /// </summary>
    /// <param name="faker">An instance of the Faker class for TModel, which can be used to configure custom rules for data generation.</param>
    /// <returns>A configured Faker instance with updated rules for generating TModel instances.</returns>
    protected virtual Faker<TModel> ConfigureFaker(Faker<TModel> faker)
        => faker;
    
    /// <summary>
    /// Adds fake models to the database for testing purposes.
    /// </summary>
    /// <param name="count">The number of models to add. Must be 1 or greater.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when count is less than 1.</exception>
    protected async Task AddFakeModelsToDbAsync(int count) {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);
        
        var faker = new Faker<TModel>();
        faker.RuleFor(m => m.Id, f => f.Random.Guid());
        faker = ConfigureFaker(faker);
        
        IEnumerable<TModel>? models = faker.GenerateLazy(count);
        await AddModelsToDbAsync(models);
    }

    /// <summary>
    /// Adds fake models to the database for testing purposes.
    /// </summary>
    /// <param name="count">The number of models to add. Must be 1 or greater.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when count is less than 1.</exception>
    protected async Task AddFakeSoftDeletedModelToDbAsync(int count) {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);
        
        var faker = new Faker<TModel>();
        faker.RuleFor(m => m.Id, f => f.Random.Guid());
        faker.RuleFor(m => m.SoftDeletedAt, f => f.Date.Past());
        faker = ConfigureFaker(faker);
        
        IEnumerable<TModel>? models = faker.GenerateLazy(count);
        await AddModelsToDbAsync(models);
    }

    /// <summary>
    /// Adds the specified model to the database.
    /// </summary>
    /// <param name="model">The model instance to add to the database. Must not be null.</param>
    /// <typeparam name="T">The type of the model. Must be a class.</typeparam>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected async Task AddModelToDbAsync<T>(T model) where T : class {
        DbSet<T> dbSet = InfiniLoreDb.Set<T>();
        await dbSet.AddAsync(model);
        await InfiniLoreDb.SaveChangesAsync();
    }

    /// <summary>
    /// Adds a collection of models to the database.
    /// </summary>
    /// <param name="models">The collection of models to be added to the database. Must not be null.</param>
    /// <typeparam name="T">The type of the model being added. Must be a reference type.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when models is null.</exception>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected async Task AddModelsToDbAsync<T>(IEnumerable<T> models) where T : class {
        DbSet<T> dbSet = InfiniLoreDb.Set<T>();
        await dbSet.AddRangeAsync(models);
        await InfiniLoreDb.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves a single model from the database using its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the model to retrieve. Must be a valid GUID.</param>
    /// <returns>The model associated with the specified identifier.</returns>
    /// <exception cref="Exception">Thrown when a model with the given identifier is not found in the database.</exception>
    protected async Task<TModel> GetModelFromDbAsync(Guid id) {
        DbSet<TModel> dbSet = InfiniLoreDb.Set<TModel>();
        TModel? model = await dbSet.FindAsync(id);
        return model ?? throw new Exception($"Model with id {id} not found.");
    }
}
