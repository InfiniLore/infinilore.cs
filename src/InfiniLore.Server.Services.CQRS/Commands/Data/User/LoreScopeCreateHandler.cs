// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Contracts.Database.Repositories.Data.User;
using InfiniLore.Server.Database.Models.Data.User;
using InfiniLore.Server.Services.CQRS.Notifications.Data.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.CQRS.Commands.Data.User;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScopeCreateHandler(IUnitOfWorkFactory unitOfWorkFactory, ILoggerFactory factory, IValidator<LoreScope> validator, IMediator mediator ) : IRequestHandler<LoreScopeCreateRequest, MediatorResponse<Guid>> {
    private readonly ILogger logger = factory.CreateLogger("LORESCOPE Create");
    
    public async Task<MediatorResponse<Guid>> Handle(LoreScopeCreateRequest request, CancellationToken ct) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);
        var userRepo = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);
        
        RepoResult loreScopeNameTakenResult = await loreScopeRepo.IsLoreScopeNameTakenAsync(request.LoreScopeName, request.OwnerId, ct);
        RepoResult userIdExistsResult = await userRepo.IsExistingIdAsync(request.OwnerId, ct);
        if (loreScopeNameTakenResult.IsSuccess) return MediatorResponse<Guid>.FromFailureString("LoreScope name already taken for this user");
        if (userIdExistsResult.IsFailure) return MediatorResponse<Guid>.FromFailureString("Owner id does not exist");
        
        // Create a new lorescope based on the request
        var loreScope = new LoreScope {
            Name = request.LoreScopeName,
            OwnerId = request.OwnerId,
            ShortDescription = request.LoreScopeDescription ?? string.Empty
        };
        
        // Validate the lorescope model
        ValidationResult validationResult = await validator.ValidateAsync(loreScope, ct);
        if (!validationResult.IsValid) {
            logger.Warning("Validation failed: {Reason}", validationResult.Errors );
            return MediatorResponse<Guid>.FromFailureString("Validation failed");
        }

        // Save to Db
        RepoResult result = await loreScopeRepo.TryAddAsync(loreScope, ct);
        if (result.IsFailure) return MediatorResponse<Guid>.FromFailureString("Failed to save user to database"); 
        
        await mediator.Publish(new NewLoreScopeCreatedNotification(loreScope.Id), ct);
        logger.LogInformation("LoreScope created: {LoreScopeId}, notification sent", loreScope.Id);
        return loreScope.Id;
    }
}
