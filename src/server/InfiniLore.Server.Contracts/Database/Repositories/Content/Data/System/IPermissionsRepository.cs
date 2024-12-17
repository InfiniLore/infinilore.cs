// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Data.System;

namespace InfiniLore.Server.Contracts.Database.Repositories.Content.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPermissionsRepository : IBaseContentRepository<InfinilorePermission> {
    public ValueTask<RepoResult> AllPermissionNamesIncludedAsync(string[] permissionNames, CancellationToken ct = default);
}
