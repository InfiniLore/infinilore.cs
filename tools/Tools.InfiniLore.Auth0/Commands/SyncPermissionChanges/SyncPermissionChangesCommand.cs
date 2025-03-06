// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using InfiniLore.Credentials.Auth0;
using InfiniLore.Credentials.Auth0.OptionConfiguration;
using InfiniLore.Credentials.Auth0.Services;
using InfiniLore.Server.Services.Auth0;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tools.InfiniLore.Auth0.Setup;

namespace Tools.InfiniLore.Auth0.Commands.SyncPermissionChanges;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[CliArgsCommand("sync-permission")]
public partial class SyncPermissionChangesCommand : ICommand<SyncPermissionChangesParameters> {
    private IServiceProvider Provider { get; } = CommandEnvironmentFactory.Create();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task ExecuteAsync(SyncPermissionChangesParameters parameters) {

        var auth0PermissionService = Provider.GetRequiredService<IAuth0PermissionService>();
        Auth0Options options = Provider.GetRequiredService<IOptions<Auth0Options>>().Value;
        var logger = Provider.GetRequiredService<ILogger<SyncPermissionChangesCommand>>();
        
        IEnumerable<string>? permissions = PermissionsStore.GetAllPermissions();
        if (permissions is null) {
            logger.Error("Permissions store is null");
            throw new Exception("Permissions store is null");
        }
        
        IEnumerable<PermissionDto> permissionsDtos = permissions
            .Select(permission => new PermissionDto(permission, string.Empty));
        
        await auth0PermissionService.SyncApiPermissionsAsync(options.Domain, permissionsDtos);

    }
}


public record PermissionDto(
    string PermissionName, 
    string Description 
) : IPermissionDto;