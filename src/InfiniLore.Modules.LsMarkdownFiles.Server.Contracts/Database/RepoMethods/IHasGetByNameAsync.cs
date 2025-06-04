// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Modules.Core.Shared.Database;

namespace InfiniLore.Server.Modules.LsMarkdownFiles.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetByNameAndOwnerAsync<T> where T : BasicModel, IHasName, IHasOwnerId {
    ValueTask<Outcome<T>> GetByNameAndOwnerAsync(string name, Guid ownerId, QueryConfig config = default, CancellationToken ct = default);
}
