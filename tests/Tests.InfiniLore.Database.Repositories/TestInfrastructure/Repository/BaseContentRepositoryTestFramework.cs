// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.InfiniLore.Database.Repositories.TestInfrastructure.Repository;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseContentRepositoryTestFramework<TRepository, TModel>(DatabaseInfrastructure infrastructure) 
    : RepositoryTestFramework<TRepository>(infrastructure) 
    where TRepository : class, IRepository, IBaseContentRepository<TModel>
    where TModel : BaseContent {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected async Task<TModel> Base_TryAddAsync_ShouldReturnSuccess(TModel model) {
        // Arrange
        Guid guid = model.Id;
        MsSqlDbContext dbContext = await UnitOfWork.GetDbContextAsync();
        
        // Act
        RepoResult result = await Repository.TryAddAsync(model);
        await dbContext.SaveChangesAsync();
        
        DbSet<TModel> dbSet = dbContext.Set<TModel>();
        TModel? permissionFromDb = await dbSet.FirstOrDefaultAsync(x => x.Id == guid);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(permissionFromDb).IsNotNull();
        await Assert.That(permissionFromDb!.Id).IsEqualTo(model.Id);

        return permissionFromDb;
    }

    protected async Task Base_TryAddAsync_ShouldFail(TModel model) {
        // Arrange
        
        // Act
        RepoResult result = await Repository.TryAddAsync(model);
        
        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.IsSuccess).IsFalse();
        await Assert.That(result.AsFailure.Value).IsEqualTo("Model already exists");
    }
}
