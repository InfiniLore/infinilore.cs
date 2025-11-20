// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FluentValidation;

namespace InfiniLore.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseModelValidator<TModel> : AbstractValidator<TModel> where TModel : BaseModel {
    protected BaseModelValidator() {
        RuleFor(model => model.Id).NotEmpty();
        RuleFor(model => model.CreatedAt).NotEmpty();
        RuleFor(model => model.ModifiedAt).NotEmpty();
    }
}
