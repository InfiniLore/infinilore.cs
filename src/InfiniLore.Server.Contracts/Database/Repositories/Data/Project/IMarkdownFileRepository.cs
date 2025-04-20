// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Database.Models.Data.Project;

namespace InfiniLore.Server.Contracts.Database.Repositories.Data.Project;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMarkdownFileRepository : IProjectDataRepository<MarkdownFile> {
    ValueTask<Result> IsFileNameTakenAsync(string name, Guid ownerId, CancellationToken ct = default);
    ValueTask<Result> IsFileNameNotTakenAsync(string name, Guid ownerId, CancellationToken ct = default);
}
