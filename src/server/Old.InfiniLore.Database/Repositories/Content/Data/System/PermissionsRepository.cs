// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Old.InfiniLore.Database.Models.Content.Account;
using Old.InfiniLore.Database.Models.Content.Data.System;
using Old.InfiniLore.Contracts.Database.Repositories.Content.Data.System;
using Old.InfiniLore.Server.Types;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Old.InfiniLore.Database.Repositories.Content.Data.System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IPermissionsRepository>(ServiceLifetime.Scoped)]
public class PermissionsRepository : BasicContentRepository<InfiniLorePermission>, IPermissionsRepository {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override Expression<Func<InfiniLorePermission, bool>> UniqueModelPredicate(InfiniLorePermission originalModel) {
        return model => model.Id == originalModel.Id
            || model.Name == originalModel.Name;
    }
    
    public async ValueTask<RepoResult<InfiniLorePermission[]>> TryGetByNamesAsync(string[] names, CancellationToken ct = default) {
        DbSet<InfiniLorePermission> permissions = GetDbContext().Permissions;

        InfiniLorePermission[] result = await permissions
            .Where(p => names.Contains(p.Name))
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }

    public async ValueTask<RepoResult<InfiniLorePermission>> TryGetByNameAsync(string name, CancellationToken ct = default) {
        DbSet<InfiniLorePermission> permissions = GetDbContext().Permissions;

        // Retrieve the permission matching the provided name
        InfiniLorePermission? permission = await permissions
            .FirstOrDefaultAsync(predicate: p => p.Name == name, ct);

        // If permission is null, return a failure result
        if (permission == null) {
            return $"Permission with name '{name}' not found.";
        }

        // Return success result with the permission
        return permission;
    }

    public async ValueTask<RepoResult<bool>> UserHasPermissionAsync(InfiniLoreUser user, InfiniLorePermission permission, CancellationToken ct = default) {
        DbSet<InfiniLorePermission> permissions = GetDbContext().Permissions;

        // Check if the user exists within the Users collection of the given permission
        bool hasPermission = await permissions
            .Where(p => p.Id == permission.Id)
            .AnyAsync(predicate: p => p.Users.Any(u => u.Id == user.Id), ct);

        // If the user has the permission, return a successful result
        return hasPermission;
    }

    public override async ValueTask<RepoResult<InfiniLorePermission[]>> TryGetAllAsync(bool reverse = false, CancellationToken ct = default) {
        DbSet<InfiniLorePermission> permissions = GetDbContext().Permissions;

        InfiniLorePermission[] result = await permissions
            .ConditionalReverse(reverse)
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }
}
