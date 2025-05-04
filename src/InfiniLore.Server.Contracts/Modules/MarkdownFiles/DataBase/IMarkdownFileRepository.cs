// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Modules.MarkdownFiles.DataBase;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMarkdownFileRepository : ILoreScopeDataRepository<IMarkdownFile> {
    ValueTask<Result> IsFileNameTakenAsync(string name, Guid ownerId, CancellationToken ct = default);
    ValueTask<Result> IsFileNameNotTakenAsync(string name, Guid ownerId, CancellationToken ct = default);
}
