// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FluentValidation;

namespace InfiniLore.Server.Modules.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BasicModelValidator<TModel> : AbstractValidator<TModel> where TModel : BasicModel {
    protected BasicModelValidator() {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage($"{nameof(BasicModel.Id)} cannot be empty. {nameof(BasicModel.Id)} must be set to a valid GUID value");
    }
}
