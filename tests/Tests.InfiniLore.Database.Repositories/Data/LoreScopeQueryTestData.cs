// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Database.Models.Content.UserData;
using System.Linq.Expressions;
using UserIdUnion=InfiniLore.Server.Contracts.Types.UserIdUnion;

namespace Tests.InfiniLore.Database.Repositories.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class LoreScopeQueryTestData {
    public static IEnumerable<LorescopeModel> GetSingleModels() {
        InfiniLoreUser user1 = InfiniLoreUserCommandTestData.GetUser1();

        yield return new LorescopeModel {
            Id = Guid.NewGuid(),
            Name = "Test Query Scope 1",
            Description = "Test Query Scope Description 1",
            Owner = user1
        };

        yield return new LorescopeModel {
            Id = Guid.NewGuid(),
            Name = "Test Query Scope 2",
            Description = "Test Query Scope Description 2",
            Owner = user1
        };
    }

    public static (UserIdUnion userId, LorescopeModel model) GetUserModels() {
        InfiniLoreUser user1 = InfiniLoreUserCommandTestData.GetUser1();

        return new(user1, new LorescopeModel {
            Id = Guid.NewGuid(),
            Name = "Test Query Scope for User 1",
            Description = "Test Query Scope Description for User 1",
            Owner = user1
        });
    }

    public static IEnumerable<LorescopeModel> GetMultipleModels() {
        InfiniLoreUser user1 = InfiniLoreUserCommandTestData.GetUser1();

        yield return new() {
            Id = Guid.NewGuid(),
            Name = "Test Query Scope 3",
            Description = "Test Query Scope Description 3",
            Owner = user1
        };

        yield return new() {
            Id = Guid.NewGuid(),
            Name = "Test Query Scope 4",
            Description = "Test Query Scope Description 4",
            Owner = user1
        };
    }

    public static (Expression<Func<LorescopeModel, bool>> expression, LorescopeModel model) GetCriteriaModels() {
        InfiniLoreUser user1 = InfiniLoreUserCommandTestData.GetUser1();

        var model = new LorescopeModel {
            Id = Guid.NewGuid(),
            Name = "Test Query Scope with Criteria",
            Description = "Test Query Scope Description with Criteria",
            Owner = user1
        };

        return new(
            scope => scope.Name == "Test Query Scope with Criteria",
            model
        );
    }
}
