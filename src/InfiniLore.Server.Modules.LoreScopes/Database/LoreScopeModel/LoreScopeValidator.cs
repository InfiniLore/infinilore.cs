// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;
using InfiniLore.Server.Modules.Core.Database;

namespace InfiniLore.Server.Modules.LoreScopes.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<IValidator<LoreScopeModel>>]
public class LoreScopeValidator : OwnedModelValidator<InfiniLoreUserModel, LoreScopeModel> {
    public LoreScopeValidator() : base() {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The Name field is required.")
            .MaximumLength(LoreScopeModel.Defaults.NameMaxLength)
            .WithMessage($"The {nameof(LoreScopeModel.Name)} cannot exceed {LoreScopeModel.Defaults.NameMaxLength} characters.");
        
        RuleFor(x => x.Description)
            .MaximumLength(LoreScopeModel.Defaults.DescriptionMaxLength)
            .WithMessage($"The {nameof(LoreScopeModel.Description)} cannot exceed {LoreScopeModel.Defaults.DescriptionMaxLength} characters.");
    }
}
