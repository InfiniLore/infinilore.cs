// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database.Repositories.Account;

namespace InfiniLore.Server.Services.Messaging.Queries.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class GetUserIdByAuth0IdHandler {

    public static async Task<MediatorResponse<Guid>> HandleAsync(
        // Message
        GetUserIdByAuth0IdQuery message,
        // Services
        IReadonlyUnitOfWorkFactory factory,
        // CT
        CancellationToken ct
    ) {
        if (message.Auth0Id.IsNullOrEmpty()) return MediatorResponse<Guid>.FromErrorString("Cannot get user id by auth0 id. Auth0 id is empty.");

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        Result<Guid> result = await userRepository.TryGetIdByAuth0IdAsync(message.Auth0Id, ct);
        if (result.IsError) return MediatorResponse<Guid>.FromErrorString("Cannot get user id by auth0 id. Auth0 id not found.");

        return result.AsSuccess;
    }
}
