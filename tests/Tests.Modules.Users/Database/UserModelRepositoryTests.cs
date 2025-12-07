// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Bogus;
using InfiniLore.Modules.Users.Database;
using Tests.Core.Database;

namespace Tests.Modules.Users.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InheritsTests]
[DiDataSource]
public class UserModelRepositoryTests(IServiceProvider serviceProvider) : BaseModelRepositoryTests<UserModelRepository, UserModel>(serviceProvider) {
    [Before(Test)]
    public async Task TestSetupAsync() => await DbSetupAsync();

    [After(Test)]
    public async Task TestTeardownAsync() => await DbTeardownAsync();

    protected override Faker<UserModel> GetConfiguredFaker() => base.GetConfiguredFaker()
        .RuleFor(model => model.UserName, (f, model) => f.Internet.UserNameUnicode(model.Id.ToString()));
}
