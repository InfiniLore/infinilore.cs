// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using MediatR;

namespace InfiniLore.Server.Services.CQRS.Queries.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetUserIdByAuth0IdHandler(IReadonlyUnitOfWorkFactory factory) : IRequestHandler<GetUserIdByAuth0IdQuery, MediatorResponse<Guid>> {

    public async Task<MediatorResponse<Guid>> Handle(GetUserIdByAuth0IdQuery request, CancellationToken ct) {
        if (request.Auth0Id.IsNullOrEmpty()) return MediatorResponse<Guid>.FromErrorString("Cannot get user id by auth0 id. Auth0 id is empty.");

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        RepoResult<Guid> result = await userRepository.TryGetIdByAuth0IdAsync(request.Auth0Id, ct);
        if (result.IsError) return MediatorResponse<Guid>.FromErrorString("Cannot get user id by auth0 id. Auth0 id not found.");

        return result.AsSuccess;
    }
}
