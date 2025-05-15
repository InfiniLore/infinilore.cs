// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Auth0.ManagementApi.Models;
using CodeOfChaos.CliArgsParser;
using InfiniLore.Credentials.Auth0.Services;
using InfiniLore.Credentials.Auth0.Utility;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Frozen;
using Tools.InfiniLore.Auth0.Setup;
using RolesStore=InfiniLore.Shared.RolesStore;

namespace Tools.InfiniLore.Auth0.Commands.SyncRoles;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[CliData("sync-roles")]
public partial class SyncRolesCommand : ICliCommand<SyncRolesParameters> {
    private IServiceProvider Provider { get; } = CommandEnvironmentFactory.Create();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask ExecuteAsync(SyncRolesParameters parameters, CancellationToken ct = default) {
        var auth0Utility = Provider.GetRequiredService<IAuth0Utility>();
        var logger = Provider.GetRequiredService<ILogger<SyncRolesCommand>>();
        var rateLimiter = Provider.GetRequiredService<IRateLimiterService>();

        FrozenDictionary<string, string[]> rolesAndPermissions = RolesStore.PermissionsPerRoles.Value;
        HashSet<string> allPermissions = rolesAndPermissions.SelectMany(role => role.Value).ToHashSet();

        // Check if auth0 is synced with our code-first permissions
        string[] auth0Permissions = await auth0Utility.Permissions.GetApiPermissionsAsync(parameters.ApiIdentifier);
        IEnumerable<string> differences = allPermissions.Except(auth0Permissions);
        if (differences.Any()) {
            logger.LogError("Permissions mismatch found. Differences: {Differences}", differences);
            logger.LogCritical("Please run 'sync-permission' command to sync permissions first.");
            return;
        }

        // Sync Roles
        IEnumerable<RoleDto> roles = RolesStore.IterateValues().Select(role => new RoleDto(role, string.Empty));
        bool result = await auth0Utility.Roles.TrySyncRolesAsync(roles);
        if (!result) {
            logger.LogError("Roles sync failed");
            return;
        }

        // Sync Permissions to Roles
        List<Role> auth0Roles = await auth0Utility.Roles.GetAllRolesAsync();
        IEnumerable<(string Id, string[] permissions)> mappedRolesToPermissions = auth0Roles.Select(static role => {
            string[] permissions = RolesStore.PermissionsPerRoles.Value[role.Name];
            return (role.Id, permissions);
        });

        // Todo increase batch size when we have more roles
        foreach ((string Id, string[] permissions)[] batch in mappedRolesToPermissions.Chunk(1)) {
            // Batch size of 1
            await rateLimiter.RetryWithRateLimit(async () => {
                foreach ((string roleId, string[] permissionIds) in batch) {
                    List<PermissionIdentity> permissions = permissionIds.Select(p => new PermissionIdentity {
                        Identifier = parameters.ApiIdentifier,
                        Name = p
                    }).ToList();

                    await auth0Utility.Roles.SyncRolePermissionsAsync(roleId, permissions);
                }
            });
        }
    }
}
