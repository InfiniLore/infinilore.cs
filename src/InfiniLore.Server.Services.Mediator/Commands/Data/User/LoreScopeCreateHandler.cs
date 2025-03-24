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
using MediatR;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.Mediator.Commands.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScopeCreateHandler(IUnitOfWorkFactory unitOfWorkFactory, ILoggerFactory factory, IValidator<LoreScope> validator, IMediator mediator) : IRequestHandler<LoreScopeCreateMediatorRequest, MediatorResponse<Guid>> {
    private readonly ILogger logger = factory.CreateLogger("LORESCOPE Create");

    public async Task<MediatorResponse<Guid>> Handle(LoreScopeCreateMediatorRequest mediatorRequest, CancellationToken ct) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);
        var userRepo = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        Result loreScopeNameTakenResult = await loreScopeRepo.IsLoreScopeNameTakenAsync(mediatorRequest.LoreScopeName, mediatorRequest.OwnerId, ct);
        Result userIdExistsResult = await userRepo.IsIdTakenAsync(mediatorRequest.OwnerId, ct);
        if (loreScopeNameTakenResult.TryGetState(out bool isTaken) && isTaken) return MediatorResponse<Guid>.FromErrorString("LoreScope name already taken for this user");
        if (userIdExistsResult.IsError) return MediatorResponse<Guid>.FromErrorString("Owner id does not exist");

        // Create a new lorescope based on the request
        var loreScope = new LoreScope {
            Name = mediatorRequest.LoreScopeName,
            OwnerId = mediatorRequest.OwnerId,
            ShortDescription = mediatorRequest.LoreScopeDescription ?? string.Empty
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

        await mediator.Publish(new NewLoreScopeCreatedNotification(loreScope.Id), ct);
        logger.LogInformation("LoreScope created: {LoreScopeId}, notification sent", loreScope.Id);
        return loreScope.Id;
    }
}
