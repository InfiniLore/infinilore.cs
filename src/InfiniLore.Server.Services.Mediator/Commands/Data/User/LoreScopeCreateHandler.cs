// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Contracts.Database.Repositories.Data.User;
using InfiniLore.Server.Database.Models.Data.User;
using InfiniLore.Server.Services.Mediator.Notifications.Data.User;
using Microsoft.Extensions.Logging;
using Wolverine;

namespace InfiniLore.Server.Services.Mediator.Commands.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScopeCreateHandler{
    public static async Task<MediatorResponse<Guid>> HandleAsync(
        // Message
        LoreScopeCreateMediatorRequest message,
        // Services
        IUnitOfWorkFactory unitOfWorkFactory, ILogger logger, IValidator<LoreScope> validator,IMessageBus messageBus,
        // CT
        CancellationToken ct
    ) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);
        var userRepo = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        Result loreScopeNameTakenResult = await loreScopeRepo.IsLoreScopeNameTakenAsync(message.LoreScopeName, message.OwnerId, ct);
        Result userIdExistsResult = await userRepo.IsIdTakenAsync(message.OwnerId, ct);
        if (loreScopeNameTakenResult.TryGetState(out bool isTaken) && isTaken) return MediatorResponse<Guid>.FromErrorString("LoreScope name already taken for this user");
        if (userIdExistsResult.IsError) return MediatorResponse<Guid>.FromErrorString("Owner id does not exist");

        // Create a new lorescope based on the request
        var loreScope = new LoreScope {
            Name = message.LoreScopeName,
            OwnerId = message.OwnerId,
            ShortDescription = message.LoreScopeDescription ?? string.Empty
        };

        // Validate the lorescope model
        ValidationResult validationResult = await validator.ValidateAsync(loreScope, ct);
        if (!validationResult.IsValid) {
            logger.Warning("Validation failed: {Reason}", validationResult.Errors);
            return MediatorResponse<Guid>.FromErrorString("Validation failed");
        }

        // Save to Db
        Result result = await loreScopeRepo.AddAsync(loreScope, ct);
        if (result.IsError) return MediatorResponse<Guid>.FromErrorString("Failed to save user to database");

        await messageBus.PublishAsync(new NewLoreScopeCreatedEvent(loreScope.Id));
        logger.LogInformation("LoreScope created: {LoreScopeId}, notification sent", loreScope.Id);
        return loreScope.Id;
    }
}
