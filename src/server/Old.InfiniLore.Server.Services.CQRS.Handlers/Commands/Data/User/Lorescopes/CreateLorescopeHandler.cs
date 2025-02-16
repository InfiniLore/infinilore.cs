// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using Old.InfiniLore.Database.Models.Content.Data.User;
using Old.InfiniLore.Contracts.Database.Repositories.Content.Data.User;
using Old.InfiniLore.Server.Services.CQRS.Requests.Commands;
using Old.InfiniLore.Server.Types;
using MediatR;

namespace Old.InfiniLore.Server.Services.CQRS.Handlers.Commands.Data.User.Lorescopes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CreateLorescopeHandler(
    IUnitOfWorkFactory unitOfWorkFactory,
    IMediator mediator  
) : IRequestHandler<CreateLorescopeCommand, SuccessOrFailure<LorescopeModel>> {

    public async Task<SuccessOrFailure<LorescopeModel>> Handle(CreateLorescopeCommand request, CancellationToken ct) {
        try {
            await using IUnitOfWork unitOfWork = await unitOfWorkFactory.CreateWithTransactionAsync(ct);
            
            var lorescopeRepository = await unitOfWork.GetRepositoryAsync<ILorescopeRepository>();

            // Pre-check if we can use the name
            // Done to get more human-readable error strings back
            RepoResult resultCanUseName = await lorescopeRepository.IsValidNewNameAsync(request.Lorescope.OwnerId, request.Lorescope.Name, ct);
            if (!resultCanUseName) {
                // await unitOfWork.TryRollbackTransactionAsync(ct); // Don't roll back because we are just retrieving data
                return resultCanUseName.AsFailure;
            } 

            // Actually add the lore scope to the db
            RepoResult<LorescopeModel> resultAddition = await lorescopeRepository.TryAddWithResultAsync(request.Lorescope, ct);
            if (!resultAddition.TryGetAsSuccess(out LorescopeModel? model)) {
                await unitOfWork.TryRollbackTransactionAsync(ct);
                return resultAddition.AsFailure;
            }

            // Everything is good
            // Because unhappy flow is already checked we can proceed with finalization
            await unitOfWork.TryCommitTransactionAsync(ct);
            // await mediator.Publish(new NewLorescopeNotification(model.Id), ct); // TODO create notification handler
            return model;
        }
        catch (Exception ex) {
            return ex.Message;
        }
    }
}

// public record NewLorescopeNotification(Guid ModelId);
