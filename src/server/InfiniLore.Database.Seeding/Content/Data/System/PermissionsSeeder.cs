// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using CodeOfChaos.Extensions.Serilog;
using InfiniLore.Database.Models.Content.Data.System;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Contracts.Database.Repositories.Content.Data.System;
using InfiniLore.Server.Contracts.Database.Seeding;
using InfiniLore.Server.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace InfiniLore.Database.Seeding.Content.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<PermissionsSeeder>(ServiceLifetime.Scoped)]
public class PermissionsSeeder(ILogger logger, IPermissionsRepository repository) : ISeeder {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task StartSeedingAsync(CancellationToken ct = default) {
        // If Permissions already exist.
        //      We can safely just ignore all of this
        string[] permissions = ApiPermissions.GetAllPermissions().ToArray();
        RepoResult<InfinilorePermission[]> getResult = await repository.TryGetByNamesAsync(permissions, ct);
        if (getResult.IsFailure) throw new Exception("Failed to get permissions.");

        InfinilorePermission[] existingPermissions = getResult.AsSuccess.Value;
        InfinilorePermission[] newPermissions = permissions
            .Except(existingPermissions.Select(p => p.Name))
            .Select(name => new InfinilorePermission { Name = name }).ToArray();

        if (newPermissions.Length == 0) {
            logger.Information("No new permissions to add.");
            return;
        }

        RepoResult result = await repository.TryAddRangeAsync(newPermissions, ct);
        if (result.IsFailure) throw new Exception("Failed to seed permissions.");
        
        logger.Information("{Count} new permissions added.", newPermissions.Length);
    }
}
