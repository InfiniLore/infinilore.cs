// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

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
    [SuppressMessage("ReSharper.DPA", "DPA0006: Large number of DB commands")]
    protected async Task<TModel> AssertModelExists(TModel model) {
        // Arrange
        Guid guid = model.Id;
        MsSqlDbContext dbContext = await UnitOfWork.GetDbContextAsync();
        dbContext.ChangeTracker.Clear();
        DbSet<TModel> dbSet = dbContext.Set<TModel>();
        
        // act
        TModel? modelFromDb = await dbSet.FirstOrDefaultAsync(x => x.Id == guid);
        
        // Assert
        await Assert.That(modelFromDb).IsNotNull();
        await Assert.That(modelFromDb!.Id).IsEqualTo(model.Id);
        
        return modelFromDb;
    }
    
    [SuppressMessage("ReSharper.DPA", "DPA0006: Large number of DB commands")]
    protected async Task AssertModelDoesNotExist(TModel model) {
        // Arrange
        Guid guid = model.Id;
        MsSqlDbContext dbContext = await UnitOfWork.GetDbContextAsync();
        dbContext.ChangeTracker.Clear();
        DbSet<TModel> dbSet = dbContext.Set<TModel>();
        
        // act
        TModel? modelFromDb = await dbSet.FirstOrDefaultAsync(x => x.Id == guid);
        
        // Assert
        await Assert.That(modelFromDb).IsNull();
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected async Task<TModel> Base_TryAddAsync_ShouldReturnSuccess(TModel model) {
        // Arrange
        Guid guid = model.Id;
        MsSqlDbContext dbContext = await UnitOfWork.GetDbContextAsync();

        // Act
        RepoResult result = await Repository.TryAddAsync(model);
        await UnitOfWork.SaveChangesAsync();

        DbSet<TModel> dbSet = dbContext.Set<TModel>();
        TModel? modelFromDb = await dbSet.FirstOrDefaultAsync(x => x.Id == guid);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(modelFromDb).IsNotNull();
        await Assert.That(modelFromDb!.Id).IsEqualTo(model.Id);

        return modelFromDb;
    }

    protected async Task Base_TryAddAsync_ShouldFail(TModel model) {
        // Arrange

        // Act
        RepoResult result = await Repository.TryAddAsync(model);
        await UnitOfWork.SaveChangesAsync();

        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.IsSuccess).IsFalse();
        await Assert.That(result.AsFailure.Value).IsEqualTo("Model already exists");
    }

    protected async Task<TModel> Base_TryAddWithResultAsync_ShouldReturnSuccess(TModel model) {
        // Arrange
        Guid guid = model.Id;

        // Act
        RepoResult<TModel> result = await Repository.TryAddWithResultAsync(model);
        bool validValue = result.TryGetSuccessValue(out TModel? outputModel);
        await UnitOfWork.SaveChangesAsync();

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(validValue).IsTrue();
        await Assert.That(outputModel).IsNotNull();
        await Assert.That(outputModel!.Id).IsEqualTo(guid);
        
        return outputModel;
    }

    protected async Task Base_TryAddRange_ShouldReturnSuccess(IEnumerable<TModel> models) {
        // Arrange
        IEnumerable<TModel> baseContents = models as TModel[] ?? models.ToArray();
        
        // Act
        RepoResult result = await Repository.TryAddRangeAsync(baseContents);
        await UnitOfWork.SaveChangesAsync();
        
        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.IsFailure).IsFalse();
        foreach (TModel model in baseContents) {
            await AssertModelExists(model);
        }
    }

    protected async Task Base_TryAddWithResultAsync_ShouldFail(TModel model) {
        // Arrange
        
        // Act
        RepoResult<TModel> result = await Repository.TryAddWithResultAsync(model);
        await UnitOfWork.SaveChangesAsync();
        
        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.IsSuccess).IsFalse();
        await Assert.That(result.AsFailure.Value).IsEqualTo("Model already exists");
    }
    
    protected async Task Base_TryUpdateAsync_ShouldReturnSuccess(TModel model) {
        // Arrange
        
        // Act
        RepoResult result = await Repository.TryUpdateAsync(model);
        await UnitOfWork.SaveChangesAsync();
        
        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await AssertModelExists(model);
    }
    
    protected async Task Base_TryUpdateAsync_ShouldFail(TModel model) {
        // Arrange
        
        // Act
        RepoResult result = await Repository.TryUpdateAsync(model);
        await UnitOfWork.SaveChangesAsync();
        
        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.IsSuccess).IsFalse();
        await Assert.That(result.AsFailure.Value).IsEqualTo("Model does not exist");
    }
    
    protected async Task<TModel> Base_TryUpdateWithResultAsync_ShouldReturnSuccess(TModel model) {
        // Arrange
        
        // Act
        RepoResult<TModel> result = await Repository.TryUpdateWithResultAsync(model);
        await UnitOfWork.SaveChangesAsync();
        bool hasValue = result.TryGetSuccessValue(out TModel? resultModel);
        
        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await AssertModelExists(model);
        await Assert.That(hasValue).IsTrue();
        await Assert.That(resultModel).IsNotNull();
        
        return resultModel!;
    }
    
    protected async Task Base_TryUpdateWithResultAsync_ShouldFail(TModel model) {
        // Arrange
        
        // Act
        RepoResult<TModel> result = await Repository.TryUpdateWithResultAsync(model);
        await UnitOfWork.SaveChangesAsync();
        
        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.IsSuccess).IsFalse();
        await Assert.That(result.AsFailure.Value).IsEqualTo("Model does not exist");
    }
    
    protected async Task Base_TryUpdateRangeAsync_ShouldReturnSuccess(IEnumerable<TModel> models) {
        // Arrange
        
        // Act
        RepoResult result = await Repository.TryUpdateAsync(models);
        await UnitOfWork.SaveChangesAsync();
        
        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.IsFailure).IsFalse();
    }
    
    protected async Task Base_TryUpdateRangeAsync_ShouldReturnFailure(IEnumerable<TModel> models) {
        // Arrange
        
        // Act
        RepoResult result = await Repository.TryUpdateAsync(models);
        await UnitOfWork.SaveChangesAsync();
        
        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.IsSuccess).IsFalse();
        await Assert.That(result.AsFailure.Value).IsEqualTo("One or more Models do not exist");
    }
    
    protected async Task<TModel> Base_TryAddOrUpdateAsync_ShouldReturnSuccess(TModel model) {
        // Arrange
        
        // Act
        RepoResult result = await Repository.TryAddOrUpdateAsync(model);
        await UnitOfWork.SaveChangesAsync();
        
        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        TModel resultModel = await AssertModelExists(model);
        return resultModel;
    }
    
    protected async Task Base_TryAddOrUpdateRangeAsync_ShouldReturnSuccess(IEnumerable<TModel> models) {
        // Arrange
        
        // Act
        IEnumerable<TModel> baseContents = models as TModel[] ?? models.ToArray();
        RepoResult result = await Repository.TryAddOrUpdateRangeAsync(baseContents);
        await UnitOfWork.SaveChangesAsync();
        
        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.IsFailure).IsFalse();
        foreach (TModel model in baseContents) {
            await AssertModelExists(model);
        }
    }
    
    protected async Task Base_TryDeleteAsync_ShouldReturnSuccess(TModel model) {
        // Arrange
        
        // Act
        RepoResult result = await Repository.TryDeleteAsync(model);
        await UnitOfWork.SaveChangesAsync();
        
        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.IsFailure).IsFalse();
        await AssertModelDoesNotExist(model);
    }

    protected async Task Base_TryDeleteAsync_ShouldReturnFailure(TModel model) {
        // Arrange
        
        // Act
        RepoResult result = await Repository.TryDeleteAsync(model);
        await UnitOfWork.SaveChangesAsync();
        
        // Assert
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.IsSuccess).IsFalse();
    }
}
