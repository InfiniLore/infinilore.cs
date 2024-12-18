// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using InfiniLore.Server.Contracts.Database.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace InfiniLore.Database.Seeding.Content.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<RolesSeeder>(ServiceLifetime.Scoped)]
public class RolesSeeder(RoleManager<IdentityRole<Guid>> roleManager, ILogger logger) : ISeeder {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task StartSeedingAsync(CancellationToken ct = default) {
        // TODO Use appropriate CQRS Handlers for seeding of : Roles
        
        IdentityRole<Guid>[] roles = [
            new("admin") { NormalizedName = "ADMIN", Id = Guid.Parse("0b3715f5-d1d2-4ffb-b869-0ab2462ad504") },
            new("user") { NormalizedName = "USER", Id = Guid.Parse("b693ab6e-5a2c-4093-947d-e1e1f3797294") }
        ];
        
        int counter = 0;
        foreach (IdentityRole<Guid> role in roles) {
            if (await roleManager.FindByNameAsync(role.Name!) is not null) {
                logger.Information("Role already exists");
                continue;
            }
            await roleManager.CreateAsync(role);
            counter++;
        }
        
        if (counter == 0) return;
        logger.Information("Created {counter} roles", counter);
    }
}
