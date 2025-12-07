// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
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
        TModel? foundModel = await repository.GetByIdAsync(knownId);

        // Assert
        await Assert.That(foundModel)
            .IsNotNull()
            .And.IsEqualTo(knownModel);
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
        TModel? foundModel = await repository.GetByIdAsync(knownId, QueryConfig.WithSoftDeleted);

        // Assert
        await Assert.That(foundModel)
            .IsNotNull()
            .And.IsEqualTo(knownModel);
    }

    [Test]
    public async Task GetByIdAsync_ShouldFail_WhenIdDoesNotExist() {
        // Arrange
        TRepository repository = await GetRepositoryAsync();
        
        // Act
        TModel? foundModel = await repository.GetByIdAsync(Guid.NewGuid());
        
        // Assert
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
        TModel? foundModel = await repository.GetByIdAsync(knownId);
        
        // Assert
        await Assert.That(foundModel).IsNull();
    }

    [Test]
    public async Task GetByIdAsync_ShouldFail_WhenIdIsEmpty() {
        // Arrange
        TRepository repository = await GetRepositoryAsync();
    
        // Act
        TModel? foundModel = await repository.GetByIdAsync(Guid.Empty);
    
        // Assert
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
        PaginatedData<TModel> paginatedData = await repository.GetAllAsync(pagination);
        
        // Assert
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
        PaginatedData<TModel> paginatedData = await repository.GetAllAsync(pagination);

        // Assert
        await Assert.That(paginatedData).IsNotNull()
            .And.HasProperty(data => data.TotalCount).IsEqualTo(totalCount)
            .HasProperty(data => data.TotalPages).IsEqualTo(1)
            .HasProperty(data => data.CurrentPage).IsEqualTo(1)
            .HasProperty(data => data.IsEmpty).IsEqualTo(false)
            .HasProperty(data => data.IsNotEmpty).IsEqualTo(true);
        
        await Assert.That(paginatedData.Items).IsNotEmpty().Count().IsEqualTo(totalCount);
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
        PaginatedData<TModel> paginatedData = await repository.GetAllAsync(pagination);

        // Assert
        await Assert.That(paginatedData).IsNotNull()
            .And.HasProperty(data => data.TotalCount).IsEqualTo(totalCount)
            .HasProperty(data => data.TotalPages).IsEqualTo(totalPages)
            .HasProperty(data => data.CurrentPage).IsEqualTo(pageNumber)
            .HasProperty(data => data.IsEmpty).IsEqualTo(false)
            .HasProperty(data => data.IsNotEmpty).IsEqualTo(true);

        await Assert.That(paginatedData.Items).IsNotEmpty().Count().IsEqualTo(pageSize);
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
        Guid foundId = await repository.AddAsync(knownModel);
        
        // Assert
        await Assert.That(foundId)
            .IsNotEmptyGuid()
            .And.IsEqualTo(knownId);
        
        await UnitOfWork.SaveChangesAsync();
        TModel foundModel = await GetModelFromDbAsync(knownId);
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
        Guid foundId = await repository.AddAsync(knownModel);
        
        // Assert
        await Assert.That(foundId)
            .IsEmptyGuid()
            .And.IsNotEqualTo(knownId);
    }
    
    [Test]
    public async Task AddAsync_ShouldFail_WhenNull() {
        // Arrange
        TRepository repository = await GetRepositoryAsync();
        
        // Act
        Guid foundId = await repository.AddAsync(null!);
        
        // Assert
        await Assert.That(foundId)
            .IsEmptyGuid();
    }
    #endregion

    #region AnyAsync
    [Test]
    public async Task AnyAsync_ShouldBeFalse_WhenNoModelsExist() {
        // Arrange
        TRepository repository = await GetRepositoryAsync();

        // Act
        bool any = await repository.AnyAsync();

        // Assert
        await Assert.That(any).IsFalse();
    }

    [Test]
    public async Task AnyAsync_ShouldBeTrue_WhenModelsExist() {
        // Arrange
        await AddFakeModelsToDbAsync(3);
        TRepository repository = await GetRepositoryAsync();

        // Act
        bool any = await repository.AnyAsync();

        // Assert
        await Assert.That(any).IsTrue();
    }

    [Test]
    public async Task AnyAsync_ShouldBeFalse_WhenOnlySoftDeletedModelsExist() {
        // Arrange
        await AddFakeSoftDeletedModelToDbAsync(3);
        TRepository repository = await GetRepositoryAsync();

        // Act
        bool any = await repository.AnyAsync();

        // Assert
        await Assert.That(any).IsFalse();
    }

    [Test]
    public async Task AnyAsync_ShouldBeTrue_WhenOnlySoftDeletedModelsExist_WithQueryConfig() {
        // Arrange
        await AddFakeSoftDeletedModelToDbAsync(3);
        TRepository repository = await GetRepositoryAsync();

        // Act
        bool any = await repository.AnyAsync(QueryConfig.WithSoftDeleted);

        // Assert
        await Assert.That(any).IsTrue();
    }
    #endregion
}
