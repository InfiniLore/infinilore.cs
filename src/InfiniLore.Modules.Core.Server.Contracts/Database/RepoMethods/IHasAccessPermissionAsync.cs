// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasAccessPermissionAsync {
    ValueTask<Shared.Outcome> HasAccessPermissionAsync(Guid resourceId, Guid userId, string permission, CancellationToken ct = default);
}
