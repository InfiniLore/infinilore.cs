// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FluentValidation;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Data.User;
using InfiniLore.Server.Database.Models.Account;
using InfiniLore.Server.Database.Models.Data.User;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using ValidationResult=FluentValidation.Results.ValidationResult;

namespace InfiniLore.Server.Services.CQRS.Commands.Data.User;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScopeCreateHandler(IUnitOfWorkFactory unitOfWorkFactory, ILoggerFactory factory, IValidator<LoreScope> validator ) : IRequestHandler<LoreScopeCreateRequest, MediatorResponse<Guid>> {
    private readonly ILogger logger = factory.CreateLogger("LORESCOPE Create");
    
    public async Task<MediatorResponse<Guid>> Handle(LoreScopeCreateRequest request, CancellationToken ct) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);
        
        RepoResult loreScopeNameTakenResult = await loreScopeRepo.IsLoreScopeNameTakenAsync(request.LoreScopeName, request.OwnerId, ct);
        if (loreScopeNameTakenResult.IsSuccess) return MediatorResponse<Guid>.FromFailureString("LoreScope name already taken");
        
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
        if (result.IsFailure) return MediatorResponse<Guid>.FromFailureString("Failed to save user to database");  ;


        // TODO send out notifications for others to pick up that a new user has been created
        return loreScope.Id;
    }
}
