// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasAccessPermissionAsync {
    ValueTask<Outcome> HasAccessPermissionAsync(Guid resourceId, Guid userId, string permission, CancellationToken ct = default);
}
