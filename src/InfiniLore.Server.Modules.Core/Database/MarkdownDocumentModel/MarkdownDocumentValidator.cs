// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IValidator<MarkdownDocumentModel>>(ServiceLifetime.Singleton)]
public class MarkdownDocumentValidator : AbstractValidator<MarkdownDocumentModel> {
    public MarkdownDocumentValidator() {
        RuleFor(x => x.Title)
            .MaximumLength(MarkdownDocumentModel.Defaults.TitleMaxLength)
            .WithMessage($"The Title cannot exceed {MarkdownDocumentModel.Defaults.TitleMaxLength} characters.");

        RuleFor(x => x.ContentHash)
            .NotEmpty().When(x => x.Content is not null)
            .WithMessage("The ContentHash field is required when Content is not null.");
    }
}
