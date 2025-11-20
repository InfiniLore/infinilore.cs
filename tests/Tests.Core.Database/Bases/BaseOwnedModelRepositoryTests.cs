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
        
        await Assert.That(foundModel).IsNotNull() // The ownedModel has tt
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
    
    // [Test]
    // public async Task GetByIdAsync_ShouldWork_WhenIdExistsAndIsSoftDeleted_WithQueryConfig() {
    //     // Arrange
    //     var knownId = Guid.NewGuid();
    //     var knownModel = new SimpleModel {
    //         Id = knownId
    //     };
    //
    //     await AddModelToDbAsync(knownModel);
    //     
    //     SimpleModelRepository repository = await GetRepositoryAsync();
    //
    //     // Act
    //     RepoOutcome<SimpleModel> outcome = await repository.GetByIdAsync(knownId, QueryConfig.IncludeSoftDeleted);
    //
    //     // Assert
    //     await Assert.That(outcome.TryGetAsSuccess(out SimpleModel? foundModel)).IsTrue();
    //     await Assert.That(foundModel).IsEqualTo(knownModel);
    // }
    //
    // [Test]
    // public async Task GetByIdAsync_ShouldFail_WhenIdDoesNotExist() {
    //     // Arrange
    //     SimpleModelRepository repository = await GetRepositoryAsync();
    //     
    //     // Act
    //     RepoOutcome<SimpleModel> outcome = await repository.GetByIdAsync(Guid.NewGuid());
    //     
    //     // Assert
    //     await Assert.That(outcome.TryGetAsSuccess(out SimpleModel? foundModel)).IsFalse();
    //     await Assert.That(foundModel).IsNull();
    // }
    //
    // [Test]
    // public async Task GetByIdAsync_ShouldFail_WhenIdDoesExistButIsSoftDeleted() {
    //     // Arrange
    //     var knownId = Guid.NewGuid();
    //     var knownModel = new SimpleModel {
    //         Id = knownId,
    //         SoftDeletedAt = DateTime.UtcNow
    //     };
    //
    //     await AddModelToDbAsync(knownModel);
    //     SimpleModelRepository repository = await GetRepositoryAsync();
    //     
    //     // Act
    //     RepoOutcome<SimpleModel> outcome = await repository.GetByIdAsync(knownId);
    //     
    //     // Assert
    //     await Assert.That(outcome.TryGetAsSuccess(out SimpleModel? foundModel)).IsFalse();
    //     await Assert.That(foundModel).IsNull();
    // }
    //
    // [Test]
    // public async Task GetByIdAsync_ShouldFail_WhenIdIsEmpty() {
    //     // Arrange
    //     SimpleModelRepository repository = await GetRepositoryAsync();
    //
    //     // Act
    //     RepoOutcome<SimpleModel> outcome = await repository.GetByIdAsync(Guid.Empty);
    //
    //     // Assert
    //     await Assert.That(outcome.TryGetAsSuccess(out SimpleModel? foundModel)).IsFalse();
    //     await Assert.That(foundModel).IsNull();
    // }
    #endregion

    #region AddAsync
    // [Test]
    // public async Task AddAsync_ShouldWork_WhenModelIsValid() {
    //     // Arrange
    //     var knownId = Guid.NewGuid();
    //     var knownModel = new SimpleModel {
    //         Id = knownId
    //     };
    //     SimpleModelRepository repository = await GetRepositoryAsync();
    //     
    //     // Act
    //     RepoOutcome outcome = await repository.AddAsync(knownModel);
    //     
    //     // Assert
    //     await Assert.That(outcome.IsSuccess).IsTrue();
    //
    //     await UnitOfWork.SaveChangesAsync();
    //     var foundModel = await GetModelFromDbAsync<SimpleModel>(knownId);
    //     await Assert.That(foundModel).IsEqualTo(knownModel);
    // }
    //
    // [Test]
    // public async Task AddAsync_ShouldFail_WhenDuplicateIdIsProvided() {
    //     // Arrange
    //     var knownId = Guid.NewGuid();
    //     var knownModel = new SimpleModel {
    //         Id = knownId
    //     };
    //     
    //     await AddModelToDbAsync(knownModel);
    //     SimpleModelRepository repository = await GetRepositoryAsync();
    //     
    //     // Act
    //     RepoOutcome outcome = await repository.AddAsync(knownModel);
    //     
    //     // Assert
    //     await Assert.That(outcome.TryGetAsError(out RepoErrorOutcome errorOutcome)).IsTrue();
    //     await Assert.That(errorOutcome.IsAlreadyExists).IsTrue();
    // }
    // [Test]
    // public async Task AddAsync_ShouldFail_WhenNull() {
    //     // Arrange
    //     SimpleModelRepository repository = await GetRepositoryAsync();
    //     
    //     // Act
    //     RepoOutcome outcome = await repository.AddAsync(null!);
    //     
    //     // Assert
    //     await Assert.That(outcome.TryGetAsError(out RepoErrorOutcome errorOutcome)).IsTrue();
    //     await Assert.That(errorOutcome.IsInvalid).IsTrue();
    // }
    #endregion
}
