// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Database.RepoMethods;
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.LoreScopes.Server;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class IAccessProtectionRulesLoreScopes {
    public static async ValueTask<bool> CanAccessRepoWithPermission<TRepo>(
        this IAccessProtectionRules rules,
        Guid loreScopeId,
        IAccessingUser access,
        string requiredPermission,
        CancellationToken ct = default
    ) where TRepo : class, IHasAccessPermissionAsync, IUnitOfWorkRepository {
        if (await rules.IsServerAsync(access, ct)) return true;
        
        await using IReadonlyUnitOfWork unitOfWork = rules.CreateReadonlyUnitOfWork();
        var repository = await unitOfWork.GetRepositoryAsync<TRepo>(ct);

        Outcome outcome = await repository.HasAccessPermissionAsync(
            loreScopeId,
            access.UserId,
            requiredPermission, 
            ct
        );
        
        if (!outcome.TryGetAsState(out bool hasAccess)) return false;
        return hasAccess;
    }
}
