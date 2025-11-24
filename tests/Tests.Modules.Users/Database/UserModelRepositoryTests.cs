// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Users.Database;
using Tests.Core.Database;

namespace Tests.Modules.Users.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InheritsTests]
[DiDataSource]
public class UserModelRepositoryTests(IServiceProvider serviceProvider) : BaseModelRepositoryTests<UserRepository, UserModel>(serviceProvider) {
    [Before(Test)]
    public async Task TestSetupAsync() => await DbSetupAsync();

    [After(Test)]
    public async Task TestTeardownAsync() => await DbTeardownAsync();
}
