// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using InfiniLore.Core.Outcomes;
using Microsoft.EntityFrameworkCore;

namespace Tests.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseModelRepositoryTests<TRepository, TModel>(IServiceProvider serviceProvider) : RepositoryTests<TRepository, TModel>(serviceProvider)
    where TModel : BaseModel, new()
    where TRepository : BaseModelRepository<TModel>, IUnitOfWorkRepository 
{
    // -----------------------------------------------------------------------------------------------------------------
    // Common Test Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region GetByIdAsync
    [Test]
    public async Task GetByIdAsync_ShouldWork_WhenIdExists() {
        // Arrange
        var knownId = Guid.NewGuid();
        var knownModel = new TModel {
            Id = knownId
        };

        await AddModelToDbAsync(knownModel);
        
        TRepository repository = await GetRepositoryAsync();

        // Act
        RepoOutcome<TModel> outcome = await repository.GetByIdAsync(knownId);

        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out TModel? foundModel)).IsTrue();
        await Assert.That(foundModel).IsEqualTo(knownModel);
    }
    
    [Test]
    public async Task GetByIdAsync_ShouldWork_WhenIdExistsAndIsSoftDeleted_WithQueryConfig() {
        // Arrange
        var knownId = Guid.NewGuid();
        var knownModel = new TModel {
            Id = knownId
        };

        await AddModelToDbAsync(knownModel);
        
        TRepository repository = await GetRepositoryAsync();

        // Act
        RepoOutcome<TModel> outcome = await repository.GetByIdAsync(knownId, QueryConfig.IncludeSoftDeleted);

        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out TModel? foundModel)).IsTrue();
        await Assert.That(foundModel).IsEqualTo(knownModel);
    }

    [Test]
    public async Task GetByIdAsync_ShouldFail_WhenIdDoesNotExist() {
        // Arrange
        TRepository repository = await GetRepositoryAsync();
        
        // Act
        RepoOutcome<TModel> outcome = await repository.GetByIdAsync(Guid.NewGuid());
        
        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out TModel? foundModel)).IsFalse();
        await Assert.That(foundModel).IsNull();
    }
    
    [Test]
    public async Task GetByIdAsync_ShouldFail_WhenIdDoesExistButIsSoftDeleted() {
        // Arrange
        var knownId = Guid.NewGuid();
        var knownModel = new TModel {
            Id = knownId,
            SoftDeletedAt = DateTime.UtcNow
        };

        await AddModelToDbAsync(knownModel);
        TRepository repository = await GetRepositoryAsync();
        
        // Act
        RepoOutcome<TModel> outcome = await repository.GetByIdAsync(knownId);
        
        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out TModel? foundModel)).IsFalse();
        await Assert.That(foundModel).IsNull();
    }

    [Test]
    public async Task GetByIdAsync_ShouldFail_WhenIdIsEmpty() {
        // Arrange
        TRepository repository = await GetRepositoryAsync();
    
        // Act
        RepoOutcome<TModel> outcome = await repository.GetByIdAsync(Guid.Empty);
    
        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out TModel? foundModel)).IsFalse();
        await Assert.That(foundModel).IsNull();
    }
    #endregion

    #region AddAsync
    [Test]
    public async Task AddAsync_ShouldWork_WhenModelIsValid() {
        // Arrange
        var knownId = Guid.NewGuid();
        var knownModel = new TModel {
            Id = knownId
        };
        TRepository repository = await GetRepositoryAsync();
        
        // Act
        RepoOutcome outcome = await repository.AddAsync(knownModel);
        
        // Assert
        await Assert.That(outcome.IsSuccess).IsTrue();

        await UnitOfWork.SaveChangesAsync();
        var foundModel = await GetModelFromDbAsync(knownId);
        await Assert.That(foundModel).IsEqualTo(knownModel);
    }
    
    [Test]
    public async Task AddAsync_ShouldFail_WhenDuplicateIdIsProvided() {
        // Arrange
        var knownId = Guid.NewGuid();
        var knownModel = new TModel {
            Id = knownId
        };
        
        await AddModelToDbAsync(knownModel);
        TRepository repository = await GetRepositoryAsync();
        
        // Act
        RepoOutcome outcome = await repository.AddAsync(knownModel);
        
        // Assert
        await Assert.That(outcome.TryGetAsError(out RepoErrorOutcome errorOutcome)).IsTrue();
        await Assert.That(errorOutcome.IsAlreadyExists).IsTrue();
    }
    
    [Test]
    public async Task AddAsync_ShouldFail_WhenNull() {
        // Arrange
        TRepository repository = await GetRepositoryAsync();
        
        // Act
        RepoOutcome outcome = await repository.AddAsync(null!);
        
        // Assert
        await Assert.That(outcome.TryGetAsError(out RepoErrorOutcome errorOutcome)).IsTrue();
        await Assert.That(errorOutcome.IsInvalid).IsTrue();
    }
    #endregion
}
