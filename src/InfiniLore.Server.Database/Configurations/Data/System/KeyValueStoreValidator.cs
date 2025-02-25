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
[InjectableService<IValidator<KeyValueStore>>(ServiceLifetime.Singleton)]
public class KeyValueStoreValidator : AbstractValidator<KeyValueStore> {
    public KeyValueStoreValidator() {
        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("The Key field is required.")
            .MaximumLength(KeyValueStore.Defaults.KeyMaxLength)
            .WithMessage($"The Key cannot exceed {KeyValueStore.Defaults.KeyMaxLength} characters.");

        RuleFor(x => x.Value)
            .NotNull().WithMessage("The Value field is required.")
            .MaximumLength(KeyValueStore.Defaults.ValueMaxLength)
            .WithMessage($"The Value cannot exceed {KeyValueStore.Defaults.ValueMaxLength} characters.");
    }
}
