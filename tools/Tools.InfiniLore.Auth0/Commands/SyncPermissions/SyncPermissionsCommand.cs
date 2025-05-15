// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using InfiniLore.Credentials.Auth0.Utility;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tools.InfiniLore.Auth0.Setup;
using PermissionsStore=InfiniLore.Shared.PermissionsStore;

namespace Tools.InfiniLore.Auth0.Commands.SyncPermissions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[CliData("sync-permission")]
public partial class SyncPermissionsCommand : ICliCommand<SyncPermissionsParameters> {
    private IServiceProvider Provider { get; } = CommandEnvironmentFactory.Create();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask ExecuteAsync(SyncPermissionsParameters parameters, CancellationToken ct = default) {
        var auth0PermissionService = Provider.GetRequiredService<IAuth0PermissionsUtility>();
        var logger = Provider.GetRequiredService<ILogger<SyncPermissionsCommand>>();

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
