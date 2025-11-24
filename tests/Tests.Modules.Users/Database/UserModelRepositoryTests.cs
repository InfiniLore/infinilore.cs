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
public class UserModelRepositoryTests(IServiceProvider serviceProvider) : BaseModelRepositoryTests<UserRepository, UserModel>(serviceProvider);
