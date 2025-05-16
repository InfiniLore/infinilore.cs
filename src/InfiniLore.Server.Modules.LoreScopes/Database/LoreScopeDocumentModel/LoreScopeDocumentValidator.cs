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
[InjectableService<IValidator<LoreScopeDocumentModel>>(ServiceLifetime.Singleton)]
public class LoreScopeDocumentValidator : AbstractValidator<LoreScopeDocumentModel> {
    public LoreScopeDocumentValidator() {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("The Content field is required.")
            .MaximumLength(LoreScopeDocumentModel.Defaults.ContentMaxLength)
            .WithMessage($"The Content cannot exceed {LoreScopeDocumentModel.Defaults.ContentMaxLength} characters.");
    }
}
