// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.LoreScopes.Database;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.LoreScopes.Messaging.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UpsertLoreScopeMetadataHandler(
    IUnitOfWorkFactory unitOfWorkFactory,
    ILogger<LoreScopeCreateHandler> logger,
    IValidator<LoreScopeModel> validator
) : CommandHandler<UpsertLoreScopeMetadataRequest, MessageResponse> {

    public override async Task<MessageResponse> ExecuteAsync(UpsertLoreScopeMetadataRequest command, CancellationToken ct = default) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);
        
        Result<LoreScopeModel> foundModelResult = await loreScopeRepo.GetByIdAsync(command.LoreScopeId, ct: ct);
        if (!foundModelResult.TryGetAsSuccess(out LoreScopeModel foundModel)) {
            logger.Warning("Failed to find lorescope with id {LoreScopeId}", command.LoreScopeId);
            return MessageResponse.FromErrorString("Failed to find lorescope with id");
        }
        
        if (command.Description.IsNotNullOrWhiteSpace()) foundModel.Description = command.Description;
        if (command.Name.IsNotNullOrWhiteSpace()) foundModel.Name = command.Name;
        foundModel.UpdateLastModifiedDate();
        
        ValidationResult? validationResult = await validator.ValidateAsync(foundModel, ct);
        if (!validationResult.IsValid) return MessageResponse.FromError(new Error<ICollection<string>>(validationResult.Errors.Select(x => x.ErrorMessage).ToList()));
        
        Result result = await loreScopeRepo.UpdateAsync(foundModel, ct);
        return !result.IsError 
            ? result.State
            : MessageResponse.FromErrorString("Failed to save lorescope to database");

    }
}
