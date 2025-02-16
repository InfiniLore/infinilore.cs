// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Old.InfiniLore.Database.Seeding.Content.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<RolesSeeder>(ServiceLifetime.Scoped)]
public class RolesSeeder(RoleManager<IdentityRole<Guid>> roleManager, ILogger logger) : Seeder {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task SeedAsync(CancellationToken ct = new()) {
        // TODO Use appropriate CQRS Handlers for seeding of : Roles
        
        IdentityRole<Guid>[] roles = [
            new("admin") { Id = Guid.Parse("0b3715f5-d1d2-4ffb-b869-0ab2462ad504") },
            new("user") { Id = Guid.Parse("b693ab6e-5a2c-4093-947d-e1e1f3797294") }
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
