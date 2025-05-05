// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;
using InfiniLore.Server.Modules.MarkdownFiles.DataBase;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.MarkdownFiles.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IValidator<IMarkdownFile>>(ServiceLifetime.Singleton)]
public class MarkdownValidator : AbstractValidator<IMarkdownFile> {
    public MarkdownValidator() {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The Name field is required.")
            .MaximumLength(MarkdownFile.Defaults.NameMaxLength)
            .WithMessage($"The Name cannot exceed {MarkdownFile.Defaults.NameMaxLength} characters.");
        
        RuleFor(x => x.Source)
            .MaximumLength(MarkdownFile.Defaults.SourceMaxLength)
            .WithMessage($"The Source cannot exceed {MarkdownFile.Defaults.SourceMaxLength} characters.");
    }
}