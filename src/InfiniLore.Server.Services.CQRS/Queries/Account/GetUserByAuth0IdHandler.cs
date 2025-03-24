// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Database.Models.Account;
using MediatR;

namespace InfiniLore.Server.Services.CQRS.Queries.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetUserByAuth0IdHandler(IReadonlyUnitOfWorkFactory factory) : IRequestHandler<GetUserByAuth0IdQuery, MediatorResponse<InfiniLoreUser>> {

    public async Task<MediatorResponse<InfiniLoreUser>> Handle(GetUserByAuth0IdQuery request, CancellationToken ct) {
        if (request.Auth0Id.IsNullOrEmpty()) return MediatorResponse<InfiniLoreUser>.FromErrorString("Cannot get user id by auth0 id. Auth0 id is empty.");

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        RepoResult<InfiniLoreUser> result = await userRepository.TryGetByAuth0IdAsync(request.Auth0Id, ct:ct);
        if (result.IsError) return MediatorResponse<InfiniLoreUser>.FromErrorString("Cannot get user id by auth0 id. Auth0 id not found.");

        return result.AsSuccess;
    }
}
