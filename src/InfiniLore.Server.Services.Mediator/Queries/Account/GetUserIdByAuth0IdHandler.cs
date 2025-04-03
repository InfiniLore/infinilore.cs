// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Server.Contracts.Database.Repositories.Account;

namespace InfiniLore.Server.Services.Mediator.Queries.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetUserIdByAuth0IdHandler(IReadonlyUnitOfWorkFactory factory) : ICommandHandler<GetUserIdByAuth0IdQuery, MediatorResponse<Guid>> {

    public async Task<MediatorResponse<Guid>> Handle(GetUserIdByAuth0IdQuery request, CancellationToken ct) {
        if (request.Auth0Id.IsNullOrEmpty()) return MediatorResponse<Guid>.FromErrorString("Cannot get user id by auth0 id. Auth0 id is empty.");

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        Result<Guid> result = await userRepository.TryGetIdByAuth0IdAsync(request.Auth0Id, ct);
        if (result.IsError) return MediatorResponse<Guid>.FromErrorString("Cannot get user id by auth0 id. Auth0 id not found.");

        return result.AsSuccess;
    }
}
