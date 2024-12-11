// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Database.Models.Content.UserData;

namespace Tests.InfiniLore.Database.Repositories.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class InfiniLoreUserCommandTestData {
    public static InfiniLoreUser GetUser1() =>
        new() {
            UserName = "User1",
            Email = "user1@example.com"
        };

    public static IEnumerable<InfiniLoreUser> GetInfiniLoreUsers() {
        // User with no scopes, multiverses, universes, or refresh tokens
        yield return GetUser1();

        // User with some scopes
        var user2 = new InfiniLoreUser {
            UserName = "User2",
            Email = "user2@example.com"
        };

        user2.Lorescopes.Add(new LorescopeModel {
            Id = Guid.NewGuid(),
            Name = "Scope1",
            Description = "Description1",
            Owner = user2
        });

        user2.Lorescopes.Add(new LorescopeModel {
            Id = Guid.NewGuid(),
            Name = "Scope2",
            Description = "Description2",
            Owner = user2
        });

        yield return user2;

        // User with some multiverses and universes
        var user3 = new InfiniLoreUser {
            UserName = "User3",
            Email = "user3@example.com"
        };

        var user3LoreScope = new LorescopeModel {
            Id = Guid.NewGuid(),
            Name = "Scope1",
            Description = "Description1",
            Owner = user3
        };

        var user3Multiverse = new MultiverseModel {
            Id = Guid.NewGuid(),
            Name = "Multiverse1",
            Description = "Description1",
            Lorescope = user3LoreScope,
            Owner = user3
        };

        var user3Universe = new UniverseModel {
            Id = Guid.NewGuid(),
            Name = "Universe1",
            Description = "Description1",
            Multiverse = user3Multiverse,
            Owner = user3
        };

        user3.Lorescopes.Add(user3LoreScope);
        user3.Multiverses.Add(user3Multiverse);
        user3.Universes.Add(user3Universe);

        yield return user3;
    }
}
