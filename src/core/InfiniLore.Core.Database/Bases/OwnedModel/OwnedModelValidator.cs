// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FluentValidation;

namespace InfiniLore.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class OwnedModelValidator<TModel, TOwner> : BaseModelValidator<TModel> 
    where TModel : OwnedModel<TOwner>
    where TOwner : BaseModel 
{
    protected OwnedModelValidator() {
        RuleFor(model => model.OwnerId).NotEmpty();
    }
}
