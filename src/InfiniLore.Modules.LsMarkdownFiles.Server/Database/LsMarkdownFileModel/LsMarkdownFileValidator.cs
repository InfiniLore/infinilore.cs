// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types.UnitOfWork;
using FluentValidation;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IValidator<LsMarkdownFileModel>>]
public class LsMarkdownFileValidator : OwnedModelValidator<LoreScopeModel, LsMarkdownFileModel> {
    private readonly IReadonlyUnitOfWorkFactory UnitOfWorkFactory;
    
    public LsMarkdownFileValidator(IReadonlyUnitOfWorkFactory unitOfWorkFactory) {
        UnitOfWorkFactory = unitOfWorkFactory;
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The Name field is required.")
            .MaximumLength(LsMarkdownFileModel.Defaults.NameMaxLength)
            .WithMessage($"The {nameof(LsMarkdownFileModel.Name)} cannot exceed {LsMarkdownFileModel.Defaults.NameMaxLength} characters.")
            .MustAsync(VerifyNameAvailabilityAsync).WithMessage(model => $"The {nameof(LsMarkdownFileModel.Name)} of {model.Name} is already taken.");
    }
    
    private async Task<bool> VerifyNameAvailabilityAsync(LsMarkdownFileModel model, string name, CancellationToken ct) {
        await using IReadonlyUnitOfWork unitOfWork = UnitOfWorkFactory.Create();
        var markdownFileRepository = await unitOfWork.GetRepositoryAsync<ILsMarkdownFileRepository>(ct);
        
        Outcome outcome = await markdownFileRepository.IsNameTakenAsync(name, model.OwnerId, model.Id, ct: ct);
        if (outcome.TryGetAsState(out bool isTaken)) return false; 
        return !isTaken;
    }
}
