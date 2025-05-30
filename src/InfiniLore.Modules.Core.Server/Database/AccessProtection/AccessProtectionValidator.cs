// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;

namespace InfiniLore.Modules.Core.Server.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IValidator<AccessProtectionModel>>]
public class AccessProtectionValidator : BasicModelValidator<AccessProtectionModel> {
    public AccessProtectionValidator() {
        RuleFor(x => x.ProtectedModelId)
            .NotEqual(Guid.Empty).WithMessage($"{nameof(AccessProtectionModel.ProtectedModelId)} cannot be empty");
        
        RuleFor(x => x.ModelOwnerId)
            .NotEqual(Guid.Empty).WithMessage($"{nameof(AccessProtectionModel.ModelOwnerId)} cannot be empty");
        
        RuleFor(x => x)
            .Must(x => x.ModelOwner == null || x.ModelOwner.Id == x.ModelOwnerId).When(x => x.ModelOwner != null)
            .WithMessage($"{nameof(AccessProtectionModel.ModelOwnerId)} must match {nameof(AccessProtectionModel.ModelOwner)}.{nameof(InfiniLoreUserModel.Id)}");
    }
}
