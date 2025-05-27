// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Server.Modules.Core.Database.RepoMethods;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasAccessPermissionAsync {
    ValueTask<Result> HasAccessPermissionAsync(Guid resourceId, Guid userId, string permission, CancellationToken ct = default);
}
