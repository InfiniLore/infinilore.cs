// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;
using InfiniLore.Server.Database.Models.Data.System;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Database.Configurations.Data.System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IValidator<KeyValueEntry>>(ServiceLifetime.Singleton)]
public class KeyValueEntryValidator : AbstractValidator<KeyValueEntry> {
    public KeyValueEntryValidator() {
        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("The Key field is required.")
            .MaximumLength(KeyValueEntry.Defaults.KeyMaxLength)
            .WithMessage($"The Key cannot exceed {KeyValueEntry.Defaults.KeyMaxLength} characters.");

        RuleFor(x => x.Value)
            .NotNull().WithMessage("The Value field is required.")
            .MaximumLength(KeyValueEntry.Defaults.ValueMaxLength)
            .WithMessage($"The Value cannot exceed {KeyValueEntry.Defaults.ValueMaxLength} characters.");
    }
}
