// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Database.Models.Account;
using MediatR;

namespace InfiniLore.Server.Services.CQRS.Queries.Account.Auth0;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Auth0UserExistHandler(IUnitOfWorkFactory factory) : IRequestHandler<Auth0UserExistQuery, bool> {

    public async Task<bool> Handle(Auth0UserExistQuery request, CancellationToken ct) {
        if (request.Auth0UserId.IsNullOrEmpty()) return false;

        await using IUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        RepoResult<InfiniLoreUser> result = await userRepository.TryGetByAuth0IdAsync(request.Auth0UserId, ct);
        return result.IsSuccess;
    }
}
