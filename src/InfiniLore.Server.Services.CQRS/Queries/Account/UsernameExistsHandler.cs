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
public class UsernameExistsHandler(IReadonlyUnitOfWorkFactory factory) : IRequestHandler<UsernameExistsQuery, MediatorResponse> {

    public async Task<MediatorResponse> Handle(UsernameExistsQuery request, CancellationToken ct) {
        if (request.Username.IsNullOrWhiteSpace()) return MediatorResponse.FromErrorString("Cannot check for username existence. Username is empty.");

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        RepoResult result = await userRepository.IsUsernameTakenAsync(request.Username, ct: ct);
        return result.IsState;
    }
}
