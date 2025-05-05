// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Modules.Core.Database.Models;
using InfiniLore.Server.Modules.LoreScopes.Database;

namespace InfiniLore.Server.Modules.MarkdownFiles.DataBase;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMarkdownFileRepository : IOwnedDataRepository<ILoreScope, IMarkdownFile> {
    ValueTask<Result> IsFileNameTakenAsync(string name, Guid ownerId, CancellationToken ct = default);
    ValueTask<Result> IsFileNameNotTakenAsync(string name, Guid ownerId, CancellationToken ct = default);
}
