// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using InfiniLore.Core.Outcomes;
using InfiniLore.Core.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Tests.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("Usage", "TUnit0059:Abstract test class with data sources requires [InheritsTests]")]
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
    
    #region GetAllAsync
    [Test]
    public async Task GetAllAsync_ShouldWork_WhenNoModelsExist() {
        // Arrange
        TRepository repository = await GetRepositoryAsync();
        var pagination = new PaginationData(1);
        
        // Act
        PaginatedRepoOutcome<TModel> outcome = await repository.GetAllAsync(pagination);
        
        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out PaginatedData<TModel>? paginatedData)).IsTrue();
        await Assert.That(paginatedData).IsNotNull()
            .And.HasProperty(data => data.Items).IsEqualTo(Array.Empty<TModel>())
            .HasProperty(data => data.TotalCount).IsEqualTo(0)
            .HasProperty(data => data.TotalPages).IsEqualTo(0)
            .HasProperty(data => data.CurrentPage).IsEqualTo(1)
            .HasProperty(data => data.IsEmpty).IsEqualTo(true);
    }

    [Test]
    public async Task GetAllAsync_ShouldWork_WhenModelsExist() {
        // Arrange
        const int totalCount = 16;
        await AddFakeModelsToDbAsync(totalCount);
        TRepository repository = await GetRepositoryAsync();
        var pagination = new PaginationData(0);

        // Act
        PaginatedRepoOutcome<TModel> outcome = await repository.GetAllAsync(pagination);

        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out PaginatedData<TModel>? paginatedData)).IsTrue();
        await Assert.That(paginatedData).IsNotNull()
            .And.HasProperty(data => data.TotalCount).IsEqualTo(totalCount)
            .HasProperty(data => data.TotalPages).IsEqualTo(1)
            .HasProperty(data => data.CurrentPage).IsEqualTo(1)
            .HasProperty(data => data.IsEmpty).IsEqualTo(false)
            .HasProperty(data => data.IsNotEmpty).IsEqualTo(true);
        
        await Assert.That(paginatedData!.Items).IsNotEmpty().Count().IsEqualTo(totalCount);
    }
    
    [Test]
    [MatrixDataSource]
    [SuppressMessage("Usage", "TUnit0300:Generic type or method may not be AOT-compatible")]
    public async Task GetAllAsync_ShouldWork_WhenMultiplePagesExist([MatrixRange<int>(0, 10)] int pageNumber) {
        // Arrange
        const int totalCount = 12 * 64;
        const int pageSize = PaginationData.DefaultPageSize;
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        await AddFakeModelsToDbAsync(totalCount);
        TRepository repository = await GetRepositoryAsync();
        var pagination = new PaginationData(pageNumber);

        // Act
        PaginatedRepoOutcome<TModel> outcome = await repository.GetAllAsync(pagination);

        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out PaginatedData<TModel>? paginatedData)).IsTrue();
        await Assert.That(paginatedData).IsNotNull()
            .And.HasProperty(data => data.TotalCount).IsEqualTo(totalCount)
            .HasProperty(data => data.TotalPages).IsEqualTo(totalPages)
            .HasProperty(data => data.CurrentPage).IsEqualTo(pageNumber)
            .HasProperty(data => data.IsEmpty).IsEqualTo(false)
            .HasProperty(data => data.IsNotEmpty).IsEqualTo(true);

        await Assert.That(paginatedData!.Items).IsNotEmpty().Count().IsEqualTo(pageSize);
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
