// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Database.Models.Account;
using MediatR;

namespace InfiniLore.Server.Services.CQRS.Queries.Account.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UserExistsByAuth0Handler(IReadonlyUnitOfWorkFactory factory) : IRequestHandler<UserExistsByAuth0Query, MediatorResponse<bool>> {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task<MediatorResponse<bool>> Handle(UserExistsByAuth0Query request, CancellationToken ct) {
        if (request.Auth0UserId.IsNullOrEmpty()) return false;
        
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        RepoResult<InfiniLoreUser> result = await userRepository.TryGetByAuth0IdAsync(request.Auth0UserId, ct);
        return result.IsSuccess;
    }
}
