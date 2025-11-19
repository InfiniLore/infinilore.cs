// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Core.Database;
using InfiniLore.Core.Outcomes;
using Tests.Core.Database.TestData;

namespace Tests.Core.Database.Bases;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[DiDataSource]
public class BaseModelRepositoryTests(InfiniLoreDbContext context, IUnitOfWork unitOfWork) : BaseRepositoryTest<SimpleModelRepository>(context, unitOfWork) {

    [Test]
    public async Task GetByIdAsync() {
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
}
