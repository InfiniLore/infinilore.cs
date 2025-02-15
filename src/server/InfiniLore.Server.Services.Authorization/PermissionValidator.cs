// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Database.Models.Content.Data.System;
using InfiniLore.Contracts.Database.Repositories.Content.Data.System;
using InfiniLore.Contracts.Services.Auth.Authorization;
using InfiniLore.Server.Types;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Collections.Concurrent;

namespace InfiniLore.Server.Services.Authorization;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IPermissionValidator>(ServiceLifetime.Scoped)]
public class PermissionValidator(IUnitOfWork unitOfWork, ILogger logger) : IPermissionValidator {
    private readonly ILogger _logger = logger.ForSectionProperty("PERMISSIONS");

    // TODO : Cache needs to be cleared / altered when a user's Permissions are updated.
    private readonly ConcurrentDictionary<InfiniLoreUser, ConcurrentDictionary<string, bool>> _userPermissionCache = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ValueTask<bool> UserHasPermission(InfiniLoreUser user, string permission, CancellationToken ct = default)
        => UserHasPermissions(user, [permission], ct);

    public async ValueTask<bool> UserHasPermissions(InfiniLoreUser user, string[] permissions, CancellationToken ct = default) {
        if (permissions.IsEmpty()) return false;
        
        bool[] results = await Task.WhenAll(permissions.Select(permission => UserHasPermissionTask(user, permission, ct)));
        return results.All(result => result);
    }

    private async Task<bool> UserHasPermissionTask(InfiniLoreUser user, string permission, CancellationToken ct = default) {
        if (CheckCache(user, permission) is {} hasPermission) return hasPermission;
        
        // Missed the cache, so we need to grab it manually

        // Check if the permission actually exists
        var permissionsRepository = await unitOfWork.GetRepositoryAsync<IPermissionsRepository>(ct);
        RepoResult<InfiniLorePermission> resultExists = await permissionsRepository.TryGetByNameAsync(permission, ct);
        if (resultExists is { IsFailure: true, AsFailure.Value: var existsFailure }) return _logger.WarningAsFalse(existsFailure);

        InfiniLorePermission permissionModel = resultExists.AsSuccess;

        // Check if the user can be found in the connection table of Permissions
        RepoResult<bool> resultHasPermission = await permissionsRepository.UserHasPermissionAsync(user, permissionModel, ct);
        if (resultHasPermission is { IsFailure: true, AsFailure.Value: var hasPermissionFailure }) return _logger.WarningAsFalse(hasPermissionFailure);

        StoreToCache(user, permission, resultHasPermission.AsSuccess);
        return resultHasPermission.AsSuccess;
    }

    // Iffy Tri-state
    private bool? CheckCache(InfiniLoreUser user, string permission) {
        if (_userPermissionCache.TryGetValue(user, out ConcurrentDictionary<string, bool>? userPermissions)
            && userPermissions.TryGetValue(permission, out bool hasPermission)
        ) {
            _logger.Debug("Cache hit for user {userId} and permission {permission}", user.Id, permission);
            return hasPermission;
        }

        // If no cache hit, return null
        _logger.Debug("Cache miss for user {userId} and permission {permission}", user.Id, permission);
        return null;
    }

    private void StoreToCache(InfiniLoreUser user, string permission, bool hasPermission) {
        // Ensure the user is added to the cache
        ConcurrentDictionary<string, bool> userPermissions = _userPermissionCache.GetOrAdd(user, valueFactory: _ => []);

        // Update or add the permission value (thread-safe)
        userPermissions.AddOrUpdate(permission, hasPermission, updateValueFactory: (_, _) => hasPermission);

        _logger.Debug("Permission '{permission}' with value '{hasPermission}' stored to cache for user {userId}", permission, hasPermission, user.Id);
    }
}
