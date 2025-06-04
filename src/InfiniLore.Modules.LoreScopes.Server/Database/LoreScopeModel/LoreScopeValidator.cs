// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types.UnitOfWork;
using FluentValidation;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Server.Modules.LoreScopes.Database;

namespace InfiniLore.Modules.LoreScopes.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IValidator<LoreScopeModel>>]
public class LoreScopeValidator : OwnedModelValidator<InfiniLoreUserModel, LoreScopeModel> {
    private readonly IReadonlyUnitOfWorkFactory UnitOfWorkFactory;
    
    public LoreScopeValidator(IReadonlyUnitOfWorkFactory unitOfWorkFactory) {
        UnitOfWorkFactory = unitOfWorkFactory;
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The Name field is required.")
            .MaximumLength(LoreScopeModel.Defaults.NameMaxLength)
            .WithMessage($"The {nameof(LoreScopeModel.Name)} cannot exceed {LoreScopeModel.Defaults.NameMaxLength} characters.")
            .MustAsync(VerifyNameAvailabilityAsync).WithMessage(model => $"The {nameof(LoreScopeModel.Name)} of {model.Name} is already taken.");
        
        RuleFor(x => x.Description)
            .MaximumLength(LoreScopeModel.Defaults.DescriptionMaxLength)
            .WithMessage($"The {nameof(LoreScopeModel.Description)} cannot exceed {LoreScopeModel.Defaults.DescriptionMaxLength} characters.");
    }
    
    private async Task<bool> VerifyNameAvailabilityAsync(LoreScopeModel model, string name, CancellationToken ct) {
        await using IReadonlyUnitOfWork unitOfWork = UnitOfWorkFactory.Create();
        var loreScopeRepository = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);
        
        Outcome outcome = await loreScopeRepository.IsNameTakenAsync(name, model.OwnerId, model.Id, ct: ct);
        if (!outcome.TryGetAsState(out bool isTaken)) return false;
        return !isTaken;
    }
}
