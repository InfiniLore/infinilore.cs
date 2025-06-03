// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LoreScopes.Messaging.Commands;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Result=InfiniLore.Modules.Core.Server.Result;

namespace InfiniLore.Modules.LoreScopes.Server.Messaging.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class UpsertLoreScopeMetadataHandler(
    IUnitOfWorkFactory unitOfWorkFactory,
    ILogger<LoreScopeCreateHandler> logger,
    IValidator<LoreScopeModel> validator
) : CommandHandler<UpsertLoreScopeMetadataRequest, Result> {

    public override async Task<Result> ExecuteAsync(UpsertLoreScopeMetadataRequest command, CancellationToken ct = default) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        AterraEngine.Unions.Result<LoreScopeModel> foundModelResult = await loreScopeRepo.GetByIdAsync(command.LoreScopeId, ct: ct);
        if (!foundModelResult.TryGetAsSuccess(out LoreScopeModel? foundModel)) {
            logger.Warning("Failed to find lorescope with id {LoreScopeId}", command.LoreScopeId);
            return Result.FromError("Failed to find lorescope with id");
        }
        
        if (command.Description.IsNotNullOrWhiteSpace()) foundModel.Description = command.Description;
        if (command.Name.IsNotNullOrWhiteSpace()) foundModel.Name = command.Name;
        foundModel.UpdateLastModifiedDate();
        
        ValidationResult? validationResult = await validator.ValidateAsync(foundModel, ct);
        if (!validationResult.IsValid) return Result.FromError(new Error<ICollection<string>>(validationResult.Errors.Select(x => x.ErrorMessage).ToList()));
        
        AterraEngine.Unions.Result result = await loreScopeRepo.UpdateAsync(foundModel, ct);
        return !result.IsError 
            ? result.State
            : Result.FromError("Failed to save lorescope to database");

    }
}
