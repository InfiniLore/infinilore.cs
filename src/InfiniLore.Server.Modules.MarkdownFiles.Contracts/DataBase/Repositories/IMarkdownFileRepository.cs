// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.MarkdownFiles.Database;

namespace InfiniLore.Server.Modules.MarkdownFiles.DataBase;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMarkdownFileRepository : IOwnedModelRepository<LoreScopeModel, MarkdownFileModel> {
    ValueTask<Result> IsFileNameTakenAsync(string name, Guid ownerId, CancellationToken ct = default);
    ValueTask<Result> IsFileNameNotTakenAsync(string name, Guid ownerId, CancellationToken ct = default);
}
