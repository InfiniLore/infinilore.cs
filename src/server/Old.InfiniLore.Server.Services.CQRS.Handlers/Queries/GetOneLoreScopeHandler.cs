// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using Old.InfiniLore.Database.Models.Content.Data.User;
using Old.InfiniLore.Contracts.Database.Repositories.Content.Data.User;
using Old.InfiniLore.Server.Services.CQRS.Requests.Queries;
using Old.InfiniLore.Server.Types;
using MediatR;
using Serilog;

namespace Old.InfiniLore.Server.Services.CQRS.Handlers.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetOneLorescopeHandler(
    IUnitOfWorkFactory unitOfWorkFactory,
    ILogger logger
) : IRequestHandler<GetOneLorescopeQuery, SuccessOrFailure<LorescopeModel>> {

    public async Task<SuccessOrFailure<LorescopeModel>> Handle(GetOneLorescopeQuery request, CancellationToken ct) {
        try {
            await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
            var lorescopeRepository = await unitOfWork.GetRepositoryAsync<ILorescopeRepository>(ct);
            
            RepoResult<LorescopeModel> result = await lorescopeRepository.TryGetByIdAsync(request.LorescopeId, ct);
            return result.ToSuccessOrFailure();
        }
        catch (Exception e) {
            logger.Error(e, "An error occurred while trying to get a lore scope");
            return "An unknown error occurred";
        }
    }
}
