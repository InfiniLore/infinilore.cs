// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FluentValidation;

namespace InfiniLore.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseOwnedModelValidator<TModel, TOwner> : BaseModelValidator<TModel> 
    where TModel : BaseOwnedModel<TOwner>
    where TOwner : BaseModel 
{
    protected BaseOwnedModelValidator() {
        RuleFor(model => model.OwnerId).NotEmpty();
    }
}
