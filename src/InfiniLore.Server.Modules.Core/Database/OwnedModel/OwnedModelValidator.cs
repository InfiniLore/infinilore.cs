// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FluentValidation;

namespace InfiniLore.Server.Modules.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class OwnedModelValidator<TOwner, TModel> : BasicModelValidator<TModel> 
    where TModel : OwnedModel<TOwner>
    where TOwner : BasicModel {
    
    protected OwnedModelValidator() {
        RuleFor(x => x.OwnerId)
            .NotEqual(Guid.Empty)
            .WithMessage($"{nameof(OwnedModel<TOwner>.OwnerId)} cannot be empty. {nameof(OwnedModel<TOwner>.OwnerId)} must be set to a valid GUID value");

        RuleFor(x => x)
            .Must(x => x.Owner == null || x.Owner.Id == x.OwnerId).When(x => x.Owner != null)
            .WithMessage($"{nameof(OwnedModel<TOwner>.OwnerId)} must match {nameof(OwnedModel<TOwner>.Owner)}.{nameof(BasicModel.Id)}");
    }
}