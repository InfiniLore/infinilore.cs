// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;
using InfiniLore.Core.Database;

namespace InfiniLore.Modules.Users.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IValidator<UserModel>>]
public class UserModelValidator: BaseModelValidator<UserModel> {
    public UserModelValidator() {
        RuleFor(model => model.UserName)
            .NotEmpty()
            .NotNull()
            .MaximumLength(256)
            .Matches("^[a-zA-Z0-9_-]+$");
    }
}
