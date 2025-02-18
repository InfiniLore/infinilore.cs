// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using InfiniLore.Server.Database.Models.Data.System;

namespace InfiniLore.Server.Database.Configurations.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IValidator<KeyValueStore>>(ServiceLifetime.Singleton)]
public class KeyValueStoreValidator : AbstractValidator<KeyValueStore> {
    public KeyValueStoreValidator() {
        // Validation rule for Key (required, maximum length: 255)
        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("The Key field is required.")
            .MaximumLength(KeyValueStore.MaxKeyLength)
            .WithMessage($"The Key cannot exceed {KeyValueStore.MaxKeyLength} characters.");

        // Validation rule for Value (optional, maximum length: 1024)
        RuleFor(x => x.Value)
            .MaximumLength(KeyValueStore.MaxValueLength)
            .WithMessage($"The Value cannot exceed {KeyValueStore.MaxValueLength} characters.")
            .When(x => x.Value != null); // Applies only if Value is not null

    }
}
