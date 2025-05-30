// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;

namespace InfiniLore.Modules.Core.Server.Database;

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

[InjectableSingleton<IValidator<BasicModel>>] 
internal sealed class BasicModelValidator : BasicModelValidator<BasicModel>;