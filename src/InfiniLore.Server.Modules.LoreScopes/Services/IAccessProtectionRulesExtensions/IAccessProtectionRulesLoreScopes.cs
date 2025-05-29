// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Modules.Core;
using InfiniLore.Server.Modules.Core.Database.RepoMethods;

namespace InfiniLore.Server.Modules.LoreScopes;

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
        if (await rules.IsServerAsync(access)) return true;
        
        await using IReadonlyUnitOfWork unitOfWork = rules.CreateReadonlyUnitOfWork();
        var repository = await unitOfWork.GetRepositoryAsync<TRepo>(ct);

        return await repository.HasAccessPermissionAsync(
            loreScopeId,
            access.UserId,
            requiredPermission, 
            ct
        );
    }
}
