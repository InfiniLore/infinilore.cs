// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Auth0.ManagementApi.Models;
using CodeOfChaos.CliArgsParser;
using InfiniLore.Credentials.Auth0.Utility;
using InfiniLore.Server.Services.Auth0;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Frozen;
using Tools.InfiniLore.Auth0.Setup;

namespace Tools.InfiniLore.Auth0.Commands.SyncRoles;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[CliArgsCommand("sync-roles")]
public partial class SyncRolesCommand : ICommand<SyncRolesParameters> {
    private IServiceProvider Provider { get; } = CommandEnvironmentFactory.Create();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task ExecuteAsync(SyncRolesParameters parameters) {
        var auth0Utility = Provider.GetRequiredService<IAuth0Utility>();
        var logger = Provider.GetRequiredService<ILogger<SyncRolesCommand>>();
        
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
            string[] permissions = RolesStore.PermissionsPerRoles.Value[role.Id];
            return (role.Id, permissions);
        });
        
        foreach ((string roleId, string[] permissionIds) in mappedRolesToPermissions) {
            List<PermissionIdentity> permissions = permissionIds.Select(p => new PermissionIdentity() {
                Identifier = parameters.ApiIdentifier,
                Name = p
            }).ToList();
            await auth0Utility.Roles.SyncRolePermissionsAsync(roleId, permissions);
            
        }
    }
}
