// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using InfiniLore.Database.Models.Content.Data.System;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Contracts.Database.Repositories.Content.Data.System;
using InfiniLore.Server.Contracts.Database.Seeding;
using InfiniLore.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace InfiniLore.Database.Seeding.Content.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<PermissionsSeeder>(ServiceLifetime.Scoped)]
public class PermissionsSeeder(ILogger logger, IUnitOfWork unitOfWork, IPermissionsRepository repository) : ISeeder {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task StartSeedingAsync(CancellationToken ct = default) {
        string[] permissions = ApiPermissions.GetAllPermissions().ToArray();
        RepoResult result = await repository.AllPermissionNamesIncludedAsync(permissions, ct); 
        if (result.IsFailure) return; // Permissions already exist.

        var dbContext = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        
        // TODO this has to become a method on the repository
        InfinilorePermission[] existingPermissions = await dbContext.Permissions.Where(p => permissions.Contains(p.Name)).ToArrayAsync(cancellationToken: ct);
        
        IEnumerable<InfinilorePermission> newPermissions = permissions
            .Except(existingPermissions.Select(p => p.Name))
            .Select(name => new InfinilorePermission { Name = name });

        result = await repository.TryAddRangeAsync(newPermissions, ct);
        if (result.IsFailure) throw new Exception("Failed to seed permissions.");
        
        await unitOfWork.SaveChangesAsync(ct);
        logger.Information("Permissions seeded.");
    }
}
