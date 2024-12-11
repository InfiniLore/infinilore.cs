// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Database.Models.Content.Data.User;

namespace Tests.InfiniLore.Database.Repositories.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class LoreScopeCommandTestData {
    public static IEnumerable<Func<LorescopeModel>> GetSingleModels() {
        // Ensure GetUser1 method exists in InfiniLoreUserTestData
        InfiniLoreUser user1 = InfiniLoreUserCommandTestData.GetUser1();

        yield return () => new LorescopeModel {
            Id = Guid.NewGuid(),
            Name = "Test Scope 1",
            Description = "Test Scope Description 1",
            Owner = user1
        };

        yield return () => new LorescopeModel {
            Id = Guid.NewGuid(),
            Name = "Test Scope 2",
            Description = "Test Scope Description 2",
            Owner = user1
        };
    }

    public static IEnumerable<Func<IEnumerable<LorescopeModel>>> GetMultipleModels() {
        InfiniLoreUser user1 = InfiniLoreUserCommandTestData.GetUser1();

        yield return () => new List<LorescopeModel> {
            new() {
                Id = Guid.NewGuid(),
                Name = "Test Scope 3",
                Description = "Test Scope Description 3",
                Owner = user1
            },
            new() {
                Id = Guid.NewGuid(),
                Name = "Test Scope 4",
                Description = "Test Scope Description 4",
                Owner = user1
            }
        };

        yield return () => [
            new LorescopeModel {
                Name = "Test Scope Without Server Side Id",
                Description = "Test Scope Description 3",
                Owner = user1
            },
            new LorescopeModel {
                Name = "Test Scope Without Server Side Id 2",
                Description = "Test Scope Description 4",
                Owner = user1
            }
        ];
    }

    public static IEnumerable<(LorescopeModel model, Func<LorescopeModel, ValueTask<LorescopeModel>> function,
        Func<LorescopeModel, bool> predicate)> GetUpdate() {
        yield return new ValueTuple<LorescopeModel, Func<LorescopeModel, ValueTask<LorescopeModel>>, Func<LorescopeModel, bool>>(
            new LorescopeModel {
                Id = Guid.NewGuid(),
                Name = "Test Scope 5",
                Description = "Test Scope Description 5",
                Owner = InfiniLoreUserCommandTestData.GetUser1()
            },
            item2: async model => {
                model.Description = "Updated Scope Description 5";
                return await Task.FromResult(model);
            },
            item3: model => model.Description == "Updated Scope Description 5"
        );
    }

    public static IEnumerable<Func<LorescopeModel>> GetDeletes() {
        yield return () => new LorescopeModel {
            Id = Guid.NewGuid(),
            Name = "Test Scope 6",
            Description = "Will be deleted",
            Owner = InfiniLoreUserCommandTestData.GetUser1()
        };
    }
}
