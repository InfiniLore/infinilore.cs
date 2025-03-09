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
public class UserExistsByAuth0Handler(IReadonlyUnitOfWorkFactory factory) : IRequestHandler<UserExistsByAuth0Query, MediatorResponse> {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task<MediatorResponse> Handle(UserExistsByAuth0Query request, CancellationToken ct) {
        if (request.Auth0UserId.IsNullOrEmpty()) return false;

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        RepoResult result = await userRepository.IsExistingAuth0Id(request.Auth0UserId, ct);
        return result.IsState;
    }
}
