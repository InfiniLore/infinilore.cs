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
[InjectableService<IValidator<KeyValueEntryModel>>(ServiceLifetime.Singleton)]
public class KeyValueEntryValidator : AbstractValidator<KeyValueEntryModel> {
    public KeyValueEntryValidator() {
        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("The Key field is required.")
            .MaximumLength(KeyValueEntryModel.Defaults.KeyMaxLength)
            .WithMessage($"The Key cannot exceed {KeyValueEntryModel.Defaults.KeyMaxLength} characters.");

        RuleFor(x => x.Value)
            .NotNull().WithMessage("The Value field is required.")
            .MaximumLength(KeyValueEntryModel.Defaults.ValueMaxLength)
            .WithMessage($"The Value cannot exceed {KeyValueEntryModel.Defaults.ValueMaxLength} characters.");
    }
}
