// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;
using InfiniLore.Server.Database.Models.Account;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Database.Configurations.Account;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IValidator<JwtRefreshTokenData>>(ServiceLifetime.Singleton)]
public class JwtRefreshTokenDataValidation : AbstractValidator<JwtRefreshTokenData> {
    public JwtRefreshTokenDataValidation() {
        // TokenHash validation: required and maximum length
        RuleFor(x => x.TokenHash)
            .NotEmpty().WithMessage("TokenHash is required.")
            .MaximumLength(JwtRefreshTokenData.MaxLengthTokenHash).WithMessage($"TokenHash must not exceed {JwtRefreshTokenData.MaxLengthTokenHash} characters.");

        // ExpiresInDays validation: must be positive
        RuleFor(x => x.ExpiresInDays)
            .GreaterThan(0).WithMessage("ExpiresInDays must be a positive number.");

        // Roles validation: cannot be null
        RuleFor(x => x.Roles)
            .NotNull().WithMessage("Roles cannot be null.");

        // Permissions validation: cannot be null
        RuleFor(x => x.Permissions)
            .NotNull().WithMessage("Permissions cannot be null.");

    }
}
