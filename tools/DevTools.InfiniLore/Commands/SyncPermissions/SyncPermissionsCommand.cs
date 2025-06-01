// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using InfiniLore.Credentials.Auth0.Utility;
using InfiniLore.Shared.Auth;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace DevTools.InfiniLore.Commands.SyncPermissions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[CliData("sync-permission")]
public partial class SyncPermissionsCommand(
    ILogger<SyncPermissionsCommand> logger,
    IAuth0PermissionsUtility auth0PermissionService
) : ICliCommand<SyncPermissionsParameters> {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask ExecuteAsync(SyncPermissionsParameters parameters, CancellationToken ct = default) {
        IEnumerable<string>? permissions = PermissionsStore.IterateValues();
        if (permissions is null) {
            logger.Error("Permissions store is null");
            throw new Exception("Permissions store is null");
        }

        IEnumerable<PermissionDto> permissionsDtos = permissions
            .Select(permission => new PermissionDto(permission, string.Empty));

        await auth0PermissionService.SyncApiPermissionsAsync(parameters.ApiIdentifier, permissionsDtos);
    }
}
