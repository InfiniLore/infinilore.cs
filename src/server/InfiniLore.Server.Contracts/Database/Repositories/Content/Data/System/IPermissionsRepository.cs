// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Data.System;

namespace InfiniLore.Server.Contracts.Database.Repositories.Content.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPermissionsRepository : IBaseContentRepository<InfiniLorePermission> {
    public ValueTask<RepoResult<InfiniLorePermission[]>> TryGetByNamesAsync(string[] names, CancellationToken ct = default);
}
