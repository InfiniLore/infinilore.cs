// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types;
using Old.InfiniLore.Database.Models.Content.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Old.InfiniLore.Database.Seeding.Content.Account;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<UserSeeder>(ServiceLifetime.Scoped)]
public class UserSeeder(UserManager<InfiniLoreUser> userManager, ILogger logger) : Seeder {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task SeedAsync(CancellationToken ct = new()) {
        // TODO Use appropriate CQRS Handlers for seeding of : Users
        
        InfiniLoreUser[] users = [
            new() {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ConcurrencyStamp = "a22a94ae-95ae-41d9-9b76-69332a675474",
                UserName = "testuser",
                Email = "testuser@example.com",
                EmailConfirmed = true,
                SecurityStamp = "d957c0f8-e90e-4068-a968-4f4b49fc165b",
                PasswordHash = "AQAAAAIAAYagAAAAEPcntIx4Y071oyt5g84a1kLZSkEA3/WG4dB8VJiyGcbZD2XUFHSqpWL9PqF+LL6aeQ=="// Test@1234
            }
        ];

        int counter = 0;
        foreach (InfiniLoreUser user in users) {
            if (userManager.FindByIdAsync(user.Id.ToString()).Result is not null) {
                logger.Information("User already exists");
                continue;
            }
            await userManager.CreateAsync(user);
            counter++;
        }

        if (counter == 0) {
            return;
        }
        
        logger.Information("Created {counter} users", counter);
    }
}
