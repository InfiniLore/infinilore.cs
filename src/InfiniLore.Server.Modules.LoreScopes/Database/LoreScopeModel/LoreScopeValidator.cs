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
            .WithMessage($"The Name cannot exceed {LoreScopeModel.Defaults.NameMaxLength} characters.");
        
        RuleFor(x => x.DescriptionId)
            .NotEqual(Guid.Empty)
            .When(x => x.Description != null)
            .WithMessage("Document ID must be set when a document is present.");

    }
}
