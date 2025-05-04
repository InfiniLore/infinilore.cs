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
[InjectableService<IValidator<LoreScope>>(ServiceLifetime.Singleton)]
public class LoreScopeValidator : AbstractValidator<LoreScope> {
    public LoreScopeValidator() {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The Name field is required.")
            .MaximumLength(LoreScope.Defaults.NameMaxLength)
            .WithMessage($"The Name cannot exceed {LoreScope.Defaults.NameMaxLength} characters.");

        RuleFor(x => x.ShortDescription)
            .MaximumLength(LoreScope.Defaults.ShortDescriptionMaxLength)
            .WithMessage($"The Short description cannot exceed {LoreScope.Defaults.ShortDescriptionMaxLength} characters.");
    }
}
