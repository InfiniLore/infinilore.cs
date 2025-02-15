// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Database.Models.Content.Data.System;
using InfiniLore.Contracts.Database.Repositories.Content.Data.System;
using InfiniLore.Server.Services;
using InfiniLore.Server.Types;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace InfiniLore.Database.Seeding.Content.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<PermissionsSeeder>(ServiceLifetime.Scoped)]
public class PermissionsSeeder(ILogger logger, IUnitOfWorkFactory unitOfWorkFactory) : Seeder {
    private Lazy<IUnitOfWork> UnitOfWork => new(unitOfWorkFactory.Create);
    private InfiniLorePermission[] CachedPermissions { get; set; } = [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------

    public override async Task<bool> ShouldSeedAsync(CancellationToken ct = default) {
        string[] permissions = ApiPermissionsStore.GetAllPermissions().ToArray();
        var repository = await UnitOfWork.Value.GetRepositoryAsync<IPermissionsRepository>(ct);
        RepoResult<InfiniLorePermission[]> getResult = await repository.TryGetByNamesAsync(permissions, ct);
        if (getResult.IsFailure) return true;

        // match a hashed map of both of the values against each-other
        HashSet<string> existingPermissions = getResult.AsSuccess.Select(p => p.Name).ToHashSet();

        CachedPermissions = permissions.ToHashSet()
            .Except(existingPermissions)
            .Select(name => new InfiniLorePermission { Name = name })
            .ToArray();

        return CachedPermissions.Length != 0;
    }

    public override async Task SeedAsync(CancellationToken ct = new()) {
        // TODO Use appropriate CQRS Handlers for seeding of : Permissions

        if (CachedPermissions.Length == 0) {
            logger.Information("No new permissions to add.");
            return;
        }
        
        var repository = await UnitOfWork.Value.GetRepositoryAsync<IPermissionsRepository>(ct);

        RepoResult result = await repository.TryAddRangeAsync(CachedPermissions, ct);
        if (result.IsFailure) throw new Exception("Failed to seed permissions.");
        
        logger.Information("{Count} new permissions added.", CachedPermissions.Length);
    }
}
