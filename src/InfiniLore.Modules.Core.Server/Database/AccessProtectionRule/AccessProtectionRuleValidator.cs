// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;

namespace InfiniLore.Modules.Core.Server.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IValidator<AccessProtectionRuleModel>>]
public class AccessProtectionRuleValidator : BasicModelValidator<AccessProtectionRuleModel> {
    public AccessProtectionRuleValidator() {
        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage($"{nameof(AccessProtectionRuleModel.UserId)} cannot be empty");
        
        RuleFor(x => x.Permission)
            .NotEmpty().WithMessage($"The {nameof(AccessProtectionRuleModel.Permission)} field is required.")
            .MaximumLength(AccessProtectionRuleModel.Defaults.PermissionMaxLength)
            .WithMessage($"The {nameof(AccessProtectionRuleModel.Permission)} field cannot exceed {AccessProtectionRuleModel.Defaults.PermissionMaxLength} characters.");
    }
}
