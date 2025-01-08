// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using CodeOfChaos.Types;
using InfiniLore.Database.Models.Content.Data.System;
using InfiniLore.Server.Contracts.Database.Repositories.Content.Data.System;
using InfiniLore.Server.Services;
using InfiniLore.Server.Types;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace InfiniLore.Database.Seeding.Content.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<PermissionsSeeder>(ServiceLifetime.Scoped)]
public class PermissionsSeeder(ILogger logger, IPermissionsRepository repository) : Seeder {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task SeedAsync(CancellationToken ct = new()) {
        // TODO Use appropriate CQRS Handlers for seeding of : Permissions
        
        // If Permissions already exist.
        //      We can safely just ignore all of this
        string[] permissions = ApiPermissions.GetAllPermissions().ToArray();
        RepoResult<InfiniLorePermission[]> getResult = await repository.TryGetByNamesAsync(permissions, ct);
        if (getResult.IsFailure) throw new Exception("Failed to get permissions.");

        InfiniLorePermission[] existingPermissions = getResult.AsSuccess;
        InfiniLorePermission[] newPermissions = permissions
            .Except(existingPermissions.Select(p => p.Name))
            .Select(name => new InfiniLorePermission { Name = name }).ToArray();

        if (newPermissions.Length == 0) {
            logger.Information("No new permissions to add.");
            return;
        }

        RepoResult result = await repository.TryAddRangeAsync(newPermissions, ct);
        if (result.IsFailure) throw new Exception("Failed to seed permissions.");
        
        logger.Information("{Count} new permissions added.", newPermissions.Length);
    }
}
