// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using InfiniLore.Core.Outcomes;
using Microsoft.EntityFrameworkCore;
using Tests.Core.Database.TestData;

namespace Tests.Core.Database.Bases;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[DiDataSource]
public class BaseOwnedModelRepositoryTests(InfiniLoreDbContext context, IUnitOfWork<InfiniLoreDbContext> unitOfWork) : BaseRepositoryTest<SimpleOwnedModelRepository>(context, unitOfWork) {

    #region GetByIdAsync
    [Test]
    public async Task GetByIdAsync_ShouldWork_WhenIdExists() {
        // Arrange
        var ownerId = Guid.NewGuid();
        var ownedId = Guid.NewGuid();

        var ownerModel = new SimpleOwnerModel {
            Id = ownerId
        };

        var ownedModel = new SimpleOwnedModel {
            Id = ownedId,
            OwnerId = ownerId
        };

        await AddModelToDbAsync(ownerModel);
        await AddModelToDbAsync(ownedModel);

        SimpleOwnedModelRepository repository = await GetRepositoryAsync();

        // Act
        RepoOutcome<SimpleOwnedModel> outcome = await repository.GetByIdAsync(ownedId);

        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out SimpleOwnedModel? foundModel)).IsTrue();

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

        var ownerModel = new SimpleOwnerModel {
            Id = ownerId
        };

        var ownedModel = new SimpleOwnedModel {
            Id = ownedId,
            OwnerId = ownerId
        };

        await AddModelToDbAsync(ownerModel);
        await AddModelToDbAsync(ownedModel);

        SimpleOwnedModelRepository repository = await GetRepositoryAsync();

        // Act
        RepoOutcome<SimpleOwnedModel> outcome = await repository.GetByIdAsync(ownedId, QueryConfig.IncludeOptionalReferences);

        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out SimpleOwnedModel? foundModel)).IsTrue();
        await Assert.That(foundModel).IsEqualTo(ownedModel);
    }

    [Test]
    public async Task GetByIdAsync_ShouldWork_WhenIdExistsAndIsSoftDeleted_WithQueryConfig() {
        // Arrange
        var ownerId = Guid.NewGuid();
        var ownedId = Guid.NewGuid();

        var ownerModel = new SimpleOwnerModel {
            Id = ownerId
        };

        var ownedModel = new SimpleOwnedModel {
            Id = ownedId,
            OwnerId = ownerId,
            SoftDeletedAt = DateTime.UtcNow
        };

        await AddModelToDbAsync(ownerModel);
        await AddModelToDbAsync(ownedModel);

        SimpleOwnedModelRepository repository = await GetRepositoryAsync();

        // Act
        RepoOutcome<SimpleOwnedModel> outcome = await repository.GetByIdAsync(ownedId, QueryConfig.IncludeSoftDeleted);

        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out SimpleOwnedModel? foundModel)).IsTrue();

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
        SimpleOwnedModelRepository repository = await GetRepositoryAsync();

        // Act
        RepoOutcome<SimpleOwnedModel> outcome = await repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out SimpleOwnedModel? foundModel)).IsFalse();
        await Assert.That(foundModel).IsNull();
    }

    [Test]
    public async Task GetByIdAsync_ShouldFail_WhenIdDoesExistButIsSoftDeleted() {
        // Arrange
        var ownerId = Guid.NewGuid();
        var ownedId = Guid.NewGuid();

        var ownerModel = new SimpleOwnerModel {
            Id = ownerId
        };

        var ownedModel = new SimpleOwnedModel {
            Id = ownedId,
            OwnerId = ownerId,
            SoftDeletedAt = DateTime.UtcNow
        };

        await AddModelToDbAsync(ownerModel);
        await AddModelToDbAsync(ownedModel);
        SimpleOwnedModelRepository repository = await GetRepositoryAsync();

        // Act
        RepoOutcome<SimpleOwnedModel> outcome = await repository.GetByIdAsync(ownedId);

        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out SimpleOwnedModel? foundModel)).IsFalse();
        await Assert.That(foundModel).IsNull();
    }

    [Test]
    public async Task GetByIdAsync_ShouldFail_WhenIdIsEmpty() {
        // Arrange
        SimpleOwnedModelRepository repository = await GetRepositoryAsync();

        // Act
        RepoOutcome<SimpleOwnedModel> outcome = await repository.GetByIdAsync(Guid.Empty);

        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out SimpleOwnedModel? foundModel)).IsFalse();
        await Assert.That(foundModel).IsNull();
    }
    #endregion

    #region AddAsync
    [Test]
    public async Task AddAsync_ShouldWork_WhenModelIsValid() {
        // Arrange
        var ownerId = Guid.NewGuid();
        var ownedId = Guid.NewGuid();

        var ownerModel = new SimpleOwnerModel {
            Id = ownerId
        };

        var ownedModel = new SimpleOwnedModel {
            Id = ownedId,
            OwnerId = ownerId
        };

        await AddModelToDbAsync(ownerModel);
        SimpleOwnedModelRepository repository = await GetRepositoryAsync();

        // Act
        RepoOutcome outcome = await repository.AddAsync(ownedModel);

        // Assert
        await Assert.That(outcome.IsSuccess).IsTrue();

        await UnitOfWork.SaveChangesAsync();
        var foundModel = await GetModelFromDbAsync<SimpleOwnedModel>(ownedId);

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

        var ownerModel = new SimpleOwnerModel {
            Id = ownerId
        };

        var ownedModel = new SimpleOwnedModel {
            Id = ownedId,
            OwnerId = ownerId
        };

        await AddModelToDbAsync(ownerModel);
        await AddModelToDbAsync(ownedModel);
        SimpleOwnedModelRepository repository = await GetRepositoryAsync();

        // Act
        RepoOutcome outcome = await repository.AddAsync(ownedModel);

        // Assert
        await Assert.That(outcome.TryGetAsError(out RepoErrorOutcome errorOutcome)).IsTrue();
        await Assert.That(errorOutcome.IsAlreadyExists).IsTrue();
    }

    [Test]
    public async Task AddAsync_ShouldFail_WhenNull() {
        // Arrange
        SimpleOwnedModelRepository repository = await GetRepositoryAsync();

        // Act
        RepoOutcome outcome = await repository.AddAsync(null!);

        // Assert
        await Assert.That(outcome.TryGetAsError(out RepoErrorOutcome errorOutcome)).IsTrue();
        await Assert.That(errorOutcome.IsInvalid).IsTrue();
    }
    #endregion
}
