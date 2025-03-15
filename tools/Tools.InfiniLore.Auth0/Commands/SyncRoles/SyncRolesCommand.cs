// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using InfiniLore.Credentials.Auth0.Services;
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
[CliArgsCommand("sync-permission")]
public partial class SyncRolesCommand : ICommand<SyncRolesParameters> {
    private IServiceProvider Provider { get; } = CommandEnvironmentFactory.Create();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task ExecuteAsync(SyncRolesParameters parameters) {
        var auth0RoleService = Provider.GetRequiredService<IAuth0RoleService>();
        var auth0PermissionService = Provider.GetRequiredService<IAuth0PermissionService>();
        var logger = Provider.GetRequiredService<ILogger<SyncRolesCommand>>();

        FrozenDictionary<string, string[]> roles = RolesStore.PermissionsPerRoles.Value;
        foreach ((string? roleName, string[]? permissionsNames) in roles) {
            auth0PermissionService.GetApiPermissionsAsync()
        }

        IEnumerable<RoleDto> permissionsDtos = permissions
            .Select(permission => new RoleDto(permission, string.Empty));

        await auth0RoleService.SyncRolePermissionsAsync(parameters.ApiIdentifier, permissionsDtos);
    }
}
