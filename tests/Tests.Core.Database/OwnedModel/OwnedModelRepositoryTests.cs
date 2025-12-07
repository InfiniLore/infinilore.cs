// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace Tests.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class OwnedModelRepositoryTests<TRepository, TModel, TOwner>(IServiceProvider serviceProvider) : RepositoryTests<TRepository, TModel>(serviceProvider)
    where TRepository : BaseModelRepository<TModel>, IUnitOfWorkRepository
    where TModel : OwnedModel<TOwner>, new()
    where TOwner : BaseModel, new() {
    
    // -----------------------------------------------------------------------------------------------------------------
    // Test Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region GetByIdAsync
    [Test]
    public async Task GetByIdAsync_ShouldWork_WhenIdExists() {
        // Arrange
        var ownerId = Guid.NewGuid();
        var ownedId = Guid.NewGuid();

        var ownerModel = new TOwner {
            Id = ownerId
        };

        var ownedModel = new TModel {
            Id = ownedId,
            OwnerId = ownerId
        };

        await AddModelToDbAsync(ownerModel);
        await AddModelToDbAsync(ownedModel);

        TRepository repository = await GetRepositoryAsync();

        // Act
        TModel? foundModel = await repository.GetByIdAsync(ownedId);

        // Assert
        await Assert.That(foundModel).IsNotNull()// The ownedModel has tt
            .And.HasProperty(model => model.OwnerId).IsEqualTo(ownerId)
            .HasProperty(model => model.Id).IsEqualTo(ownedId)
            .HasProperty(model => model.IsSoftDeleted).IsEqualTo(ownedModel.IsSoftDeleted)
            .HasProperty(model => model.SoftDeletedAt).IsEqualTo(ownedModel.SoftDeletedAt)
            .HasProperty(model => model.CreatedAt).IsEqualTo(ownedModel.CreatedAt)
            .HasProperty(model => model.ModifiedAt).IsEqualTo(ownedModel.ModifiedAt);
    }

    [Test]
    public async Task GetByIdAsync_ShouldWork_WhenIdExists_WithOptionalInclude() {
        // Arrange
        var ownerId = Guid.NewGuid();
        var ownedId = Guid.NewGuid();

        var ownerModel = new TOwner {
            Id = ownerId
        };

        var ownedModel = new TModel {
            Id = ownedId,
            OwnerId = ownerId
        };

        await AddModelToDbAsync(ownerModel);
        await AddModelToDbAsync(ownedModel);

        TRepository repository = await GetRepositoryAsync();

        // Act
        TModel? foundModel = await repository.GetByIdAsync(ownedId, QueryConfig.WithOptionalInclude);

        // Assert
        await Assert.That(foundModel).IsEqualTo(ownedModel);
    }

    [Test]
    public async Task GetByIdAsync_ShouldWork_WhenIdExistsAndIsSoftDeleted_WithQueryConfig() {
        // Arrange
        var ownerId = Guid.NewGuid();
        var ownedId = Guid.NewGuid();

        var ownerModel = new TOwner {
            Id = ownerId
        };

        var ownedModel = new TModel {
            Id = ownedId,
            OwnerId = ownerId,
            SoftDeletedAt = DateTime.UtcNow
        };

        await AddModelToDbAsync(ownerModel);
        await AddModelToDbAsync(ownedModel);

        TRepository repository = await GetRepositoryAsync();

        // Act
        TModel? foundModel = await repository.GetByIdAsync(ownedId, QueryConfig.WithSoftDeleted);

        // Assert
        await Assert.That(foundModel).IsNotNull()
            .And.HasProperty(model => model.OwnerId).IsEqualTo(ownerId)
            .HasProperty(model => model.Id).IsEqualTo(ownedId)
            .HasProperty(model => model.IsSoftDeleted).IsEqualTo(ownedModel.IsSoftDeleted)
            .HasProperty(model => model.SoftDeletedAt).IsEqualTo(ownedModel.SoftDeletedAt)
            .HasProperty(model => model.CreatedAt).IsEqualTo(ownedModel.CreatedAt)
            .HasProperty(model => model.ModifiedAt).IsEqualTo(ownedModel.ModifiedAt);
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
        var ownerId = Guid.NewGuid();
        var ownedId = Guid.NewGuid();

        var ownerModel = new TOwner {
            Id = ownerId
        };

        var ownedModel = new TModel {
            Id = ownedId,
            OwnerId = ownerId,
            SoftDeletedAt = DateTime.UtcNow
        };

        await AddModelToDbAsync(ownerModel);
        await AddModelToDbAsync(ownedModel);
        TRepository repository = await GetRepositoryAsync();

        // Act
        TModel? foundModel = await repository.GetByIdAsync(ownedId);

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

    #region AddAsync
    [Test]
    public async Task AddAsync_ShouldWork_WhenModelIsValid() {
        // Arrange
        var ownerId = Guid.NewGuid();
        var ownedId = Guid.NewGuid();

        var ownerModel = new TOwner {
            Id = ownerId
        };

        var ownedModel = new TModel {
            Id = ownedId,
            OwnerId = ownerId
        };

        await AddModelToDbAsync(ownerModel);
        TRepository repository = await GetRepositoryAsync();

        // Act
        Guid foundId = await repository.AddAsync(ownedModel);

        // Assert
        await Assert.That(foundId)
            .IsNotEmptyGuid()
            .And.IsEqualTo(ownedId);
        
        await UnitOfWork.SaveChangesAsync();
        TModel foundModel = await GetModelFromDbAsync(ownedId);

        await Assert.That(foundModel).IsNotNull()
            .And.HasProperty(model => model.OwnerId).IsEqualTo(ownerId)
            .HasProperty(model => model.Id).IsEqualTo(ownedId)
            .HasProperty(model => model.IsSoftDeleted).IsEqualTo(ownedModel.IsSoftDeleted)
            .HasProperty(model => model.SoftDeletedAt).IsEqualTo(ownedModel.SoftDeletedAt)
            .HasProperty(model => model.CreatedAt).IsEqualTo(ownedModel.CreatedAt)
            .HasProperty(model => model.ModifiedAt).IsEqualTo(ownedModel.ModifiedAt);
    }

    [Test]
    public async Task AddAsync_ShouldFail_WhenDuplicateIdIsProvided() {
        // Arrange
        var ownerId = Guid.NewGuid();
        var ownedId = Guid.NewGuid();

        var ownerModel = new TOwner {
            Id = ownerId
        };

        var ownedModel = new TModel {
            Id = ownedId,
            OwnerId = ownerId
        };

        await AddModelToDbAsync(ownerModel);
        await AddModelToDbAsync(ownedModel);
        TRepository repository = await GetRepositoryAsync();

        // Act
        Guid foundId = await repository.AddAsync(ownedModel);

        // Assert
        await Assert.That(foundId).IsEmptyGuid();
    }
    #endregion
}
