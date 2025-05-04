// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Server.Contracts.Database.Repositories.Data.User;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LoreScopes.Messaging.Noticiations;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.LoreScopes.Messaging.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class LoreScopeCreateHandler(IUnitOfWorkFactory unitOfWorkFactory, ILogger<LoreScopeCreateHandler> logger, IValidator<LoreScope> validator) : CommandHandler<LoreScopeCreateRequest, MessageResponse<Guid>> {
    public override async Task<MessageResponse<Guid>> ExecuteAsync(LoreScopeCreateRequest command, CancellationToken ct = new()) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);
        var userRepo = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        Result loreScopeNameTakenResult = await loreScopeRepo.IsLoreScopeNameTakenAsync(command.LoreScopeName, command.OwnerId, ct);
        Result userIdExistsResult = await userRepo.IsIdTakenAsync(command.OwnerId, ct);
        if (loreScopeNameTakenResult.TryGetState(out bool isTaken) && isTaken) return MessageResponse<Guid>.FromErrorString("LoreScope name already taken for this user");
        if (userIdExistsResult.IsError) return MessageResponse<Guid>.FromErrorString("Owner id does not exist");

        // Create a new lorescope based on the request
        var loreScope = new LoreScope {
            Name = command.LoreScopeName,
            OwnerId = command.OwnerId,
            ShortDescription = command.LoreScopeDescription ?? string.Empty
        };

        // Validate the lorescope model
        ValidationResult validationResult = await validator.ValidateAsync(loreScope, ct);
        if (!validationResult.IsValid) {
            logger.Warning("Validation failed: {Reason}", validationResult.Errors);
            return MessageResponse<Guid>.FromErrorString("Validation failed");
        }

        // Save to Db
        Result result = await loreScopeRepo.AddAsync(loreScope, ct);
        if (result.IsError) return MessageResponse<Guid>.FromErrorString("Failed to save user to database");

        await new NewLoreScopeCreatedEvent(loreScope.Id).PublishAsync(Mode.WaitForAll, ct);
        logger.LogInformation("LoreScope created: {LoreScopeId}, notification sent", loreScope.Id);
        return loreScope.Id;
    }
}
