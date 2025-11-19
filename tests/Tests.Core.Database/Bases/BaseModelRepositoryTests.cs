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
public class BaseModelRepositoryTests(InfiniLoreDbContext context, IUnitOfWork<InfiniLoreDbContext> unitOfWork) : BaseRepositoryTest<SimpleModelRepository>(context, unitOfWork) {

    [Test]
    public async Task GetByIdAsync_ShouldWork_WhenIdExists() {
        // Arrange
        var knownId = Guid.NewGuid();
        var knownModel = new SimpleModel {
            Id = knownId
        };

        await AddModelToDbAsync(knownModel);
        
        SimpleModelRepository repository = await GetRepositoryAsync();

        // Act
        RepoOutcome<SimpleModel> outcome = await repository.GetByIdAsync(knownId);

        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out SimpleModel? foundModel)).IsTrue();
        await Assert.That(foundModel).IsEqualTo(knownModel);
    }
    
    [Test]
    public async Task GetByIdAsync_ShouldWork_WhenIdExistsAndIsSoftDeleted_WithQueryConfig() {
        // Arrange
        var knownId = Guid.NewGuid();
        var knownModel = new SimpleModel {
            Id = knownId
        };

        await AddModelToDbAsync(knownModel);
        
        SimpleModelRepository repository = await GetRepositoryAsync();

        // Act
        RepoOutcome<SimpleModel> outcome = await repository.GetByIdAsync(knownId, QueryConfig.IncludeSoftDeleted);

        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out SimpleModel? foundModel)).IsTrue();
        await Assert.That(foundModel).IsEqualTo(knownModel);
    }

    [Test]
    public async Task GetByIdAsync_ShouldFail_WhenIdDoesNotExist() {
        // Arrange
        SimpleModelRepository repository = await GetRepositoryAsync();
        
        // Act
        RepoOutcome<SimpleModel> outcome = await repository.GetByIdAsync(Guid.NewGuid());
        
        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out SimpleModel? foundModel)).IsFalse();
        await Assert.That(foundModel).IsNull();
    }
    
    [Test]
    public async Task GetByIdAsync_ShouldFail_WhenIdDoesExistButIsSoftDeleted() {
        // Arrange
        var knownId = Guid.NewGuid();
        var knownModel = new SimpleModel {
            Id = knownId,
            SoftDeletedAt = DateTime.UtcNow
        };

        await AddModelToDbAsync(knownModel);
        SimpleModelRepository repository = await GetRepositoryAsync();
        
        // Act
        RepoOutcome<SimpleModel> outcome = await repository.GetByIdAsync(knownId);
        
        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out SimpleModel? foundModel)).IsFalse();
        await Assert.That(foundModel).IsNull();
    }
}
