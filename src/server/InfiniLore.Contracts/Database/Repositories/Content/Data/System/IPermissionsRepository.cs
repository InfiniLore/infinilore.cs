// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Database.Models.Content.Data.System;
using InfiniLore.Server.Types;

namespace InfiniLore.Contracts.Database.Repositories.Content.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPermissionsRepository : IBasicContentRepository<InfiniLorePermission> {
    public ValueTask<RepoResult<InfiniLorePermission[]>> TryGetByNamesAsync(string[] names, CancellationToken ct = default);
    public ValueTask<RepoResult<InfiniLorePermission>> TryGetByNameAsync(string names, CancellationToken ct = default);

    public ValueTask<RepoResult<bool>> UserHasPermissionAsync(InfiniLoreUser user, InfiniLorePermission permission, CancellationToken ct = default);
}
