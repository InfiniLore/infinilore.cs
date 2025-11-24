// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FluentValidation;
using InfiniLore.Modules.Users.Database;
using Tests.Core.Database;

namespace Tests.Modules.Users.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[DiDataSource]
[InheritsTests]
public class UserModelValidatorTests(IValidator<UserModel> validator) : BaseModelValidatorTests<UserModel>(validator) {

    public override IEnumerable<Func<(UserModel, bool)>> TestCases()  {
        yield return () => (new UserModel {UserName = "AnnaSasDev"}, true);
        yield return () => (new UserModel { Id = Guid.Empty, UserName = string.Empty}, false);
        yield return () => (new UserModel { UserName = string.Empty }, false);
        yield return () => (new UserModel { UserName = "#INVALID!!!" }, false);
    }
}
