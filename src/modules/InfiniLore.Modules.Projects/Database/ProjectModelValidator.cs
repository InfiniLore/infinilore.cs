// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;
using InfiniLore.Core.Database;
using InfiniLore.Modules.Users.Database;

namespace InfiniLore.Modules.Projects.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IValidator<ProjectModel>>]
public class ProjectModelValidator : OwnedModelValidator<ProjectModel, UserModel> {
    public ProjectModelValidator() {
        RuleFor(model => model.Name)
            .NotEmpty()
            .NotNull()
            .MaximumLength(256);
    }
}
