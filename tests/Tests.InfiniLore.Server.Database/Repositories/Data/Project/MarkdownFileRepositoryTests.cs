// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using DataSources.InfiniLore.Server;
using Fakers.InfiniLore.Server;
using InfiniLore.Server.Contracts.Database.Repositories.Data.Project;
using InfiniLore.Server.Database.Models.Data.Project;
using InfiniLore.Server.Database.Repositories.Data.Project;

namespace Tests.InfiniLore.Server.Database.Repositories.Data.Project;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[ClassDataSource<ContentDbInfrastructure, MarkdownFileFaker, GuidStore>(Shared = [SharedType.PerTestSession, SharedType.PerClass])]
public class MarkdownFileRepositoryTests(ContentDbInfrastructure infrastructure, MarkdownFileFaker faker, GuidStore guidStore) {
    [Test]
    public async Task BoundToCorrectRepository() {
        // Arrange
        await using IUnitOfWork unitOfWork = infrastructure.GetReadonlyUnitOfWork();

        // Act
        var repo = await unitOfWork.GetRepositoryAsync<IMarkdownFileRepository>();

        // Assert
        await Assert.That(repo).IsTypeOf<MarkdownFileRepository>();
    }
    
    [Test]
    public async Task CanCreateMarkdownFile() {
        // Arrange
        await using IUnitOfWork unitOfWork = await infrastructure.GetUnitOfWork();
        var repo = await unitOfWork.GetRepositoryAsync<IMarkdownFileRepository>();
        MarkdownFile markdownFile = faker.GetById(guidStore.GetGuid(1000),guidStore.GetGuid("lorescope-forUser2"));
        
        // Act
        Result result = await repo.AddAsync(markdownFile);

        // Assert
        await Assert.That(result.TryGetState(out bool isSuccess)).IsTrue();
        await Assert.That(isSuccess).IsTrue();
    }
}
