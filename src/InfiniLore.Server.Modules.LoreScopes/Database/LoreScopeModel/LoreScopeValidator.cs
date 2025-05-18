// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.LoreScopes.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IValidator<LoreScopeModel>>(ServiceLifetime.Singleton)]
public class LoreScopeValidator : AbstractValidator<LoreScopeModel> {
    public LoreScopeValidator() {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The Name field is required.")
            .MaximumLength(LoreScopeModel.Defaults.NameMaxLength)
            .WithMessage($"The {nameof(LoreScopeModel.Name)} cannot exceed {LoreScopeModel.Defaults.NameMaxLength} characters.");
        
        RuleFor(x => x.Description)
            .MaximumLength(LoreScopeModel.Defaults.DescriptionMaxLength)
            .WithMessage($"The {nameof(LoreScopeModel.Description)} cannot exceed {LoreScopeModel.Defaults.DescriptionMaxLength} characters.");

    }
}
