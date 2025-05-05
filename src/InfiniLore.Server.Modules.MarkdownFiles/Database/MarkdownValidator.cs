// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.MarkdownFiles.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IValidator<MarkdownFileModel>>(ServiceLifetime.Singleton)]
public class MarkdownValidator : AbstractValidator<MarkdownFileModel> {
    public MarkdownValidator() {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The Name field is required.")
            .MaximumLength(MarkdownFileModel.Defaults.NameMaxLength)
            .WithMessage($"The Name cannot exceed {MarkdownFileModel.Defaults.NameMaxLength} characters.");
        
        RuleFor(x => x.Source)
            .MaximumLength(MarkdownFileModel.Defaults.SourceMaxLength)
            .WithMessage($"The Source cannot exceed {MarkdownFileModel.Defaults.SourceMaxLength} characters.");
    }
}