// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using InfiniLore.Core.Outcomes;
using InfiniLore.Modules.Users.Database;
using Microsoft.EntityFrameworkCore;
using Tests.Core.Database.TestData;

namespace Tests.Modules.Users.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[DiDataSource]
public class UserModelRepositoryTests(InfiniLoreDb context, IUnitOfWork<InfiniLoreDb> unitOfWork) : BaseRepositoryTest<UserRepository>(context, unitOfWork) {

    #region GetByIdAsync
    [Test]
    public async Task GetByIdAsync_ShouldWork_WhenIdExists() {
        // Arrange
        var knownId = Guid.NewGuid();
        var knownModel = new UserModel {
            Id = knownId
        };

        await AddModelToDbAsync(knownModel);
        
        UserRepository repository = await GetRepositoryAsync();

        // Act
        RepoOutcome<UserModel> outcome = await repository.GetByIdAsync(knownId);

        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out UserModel? foundModel)).IsTrue();
        await Assert.That(foundModel).IsEqualTo(knownModel);
    }
    
    [Test]
    public async Task GetByIdAsync_ShouldWork_WhenIdExistsAndIsSoftDeleted_WithQueryConfig() {
        // Arrange
        var knownId = Guid.NewGuid();
        var knownModel = new UserModel {
            Id = knownId
        };

        await AddModelToDbAsync(knownModel);
        
        UserRepository repository = await GetRepositoryAsync();

        // Act
        RepoOutcome<UserModel> outcome = await repository.GetByIdAsync(knownId, QueryConfig.IncludeSoftDeleted);

        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out UserModel? foundModel)).IsTrue();
        await Assert.That(foundModel).IsEqualTo(knownModel);
    }

    [Test]
    public async Task GetByIdAsync_ShouldFail_WhenIdDoesNotExist() {
        // Arrange
        UserRepository repository = await GetRepositoryAsync();
        
        // Act
        RepoOutcome<UserModel> outcome = await repository.GetByIdAsync(Guid.NewGuid());
        
        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out UserModel? foundModel)).IsFalse();
        await Assert.That(foundModel).IsNull();
    }
    
    [Test]
    public async Task GetByIdAsync_ShouldFail_WhenIdDoesExistButIsSoftDeleted() {
        // Arrange
        var knownId = Guid.NewGuid();
        var knownModel = new UserModel {
            Id = knownId,
            SoftDeletedAt = DateTime.UtcNow
        };

        await AddModelToDbAsync(knownModel);
        UserRepository repository = await GetRepositoryAsync();
        
        // Act
        RepoOutcome<UserModel> outcome = await repository.GetByIdAsync(knownId);
        
        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out UserModel? foundModel)).IsFalse();
        await Assert.That(foundModel).IsNull();
    }

    [Test]
    public async Task GetByIdAsync_ShouldFail_WhenIdIsEmpty() {
        // Arrange
        UserRepository repository = await GetRepositoryAsync();
    
        // Act
        RepoOutcome<UserModel> outcome = await repository.GetByIdAsync(Guid.Empty);
    
        // Assert
        await Assert.That(outcome.TryGetAsSuccess(out UserModel? foundModel)).IsFalse();
        await Assert.That(foundModel).IsNull();
    }
    #endregion

    #region AddAsync
    [Test]
    public async Task AddAsync_ShouldWork_WhenModelIsValid() {
        // Arrange
        var knownId = Guid.NewGuid();
        var knownModel = new UserModel {
            Id = knownId
        };
        UserRepository repository = await GetRepositoryAsync();
        
        // Act
        RepoOutcome outcome = await repository.AddAsync(knownModel);
        
        // Assert
        await Assert.That(outcome.IsSuccess).IsTrue();

        await UnitOfWork.SaveChangesAsync();
        var foundModel = await GetModelFromDbAsync<UserModel>(knownId);
        await Assert.That(foundModel).IsEqualTo(knownModel);
    }
    
    [Test]
    public async Task AddAsync_ShouldFail_WhenDuplicateIdIsProvided() {
        // Arrange
        var knownId = Guid.NewGuid();
        var knownModel = new UserModel {
            Id = knownId
        };
        
        await AddModelToDbAsync(knownModel);
        UserRepository repository = await GetRepositoryAsync();
        
        // Act
        RepoOutcome outcome = await repository.AddAsync(knownModel);
        
        // Assert
        await Assert.That(outcome.TryGetAsError(out RepoErrorOutcome errorOutcome)).IsTrue();
        await Assert.That(errorOutcome.IsAlreadyExists).IsTrue();
    }
    [Test]
    public async Task AddAsync_ShouldFail_WhenNull() {
        // Arrange
        UserRepository repository = await GetRepositoryAsync();
        
        // Act
        RepoOutcome outcome = await repository.AddAsync(null!);
        
        // Assert
        await Assert.That(outcome.TryGetAsError(out RepoErrorOutcome errorOutcome)).IsTrue();
        await Assert.That(errorOutcome.IsInvalid).IsTrue();
    }
    #endregion
}
