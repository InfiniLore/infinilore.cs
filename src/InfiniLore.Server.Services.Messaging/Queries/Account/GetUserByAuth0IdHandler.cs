// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Database.Models.Account;
using JetBrains.Annotations;

namespace InfiniLore.Server.Services.Messaging.Queries.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetUserByAuth0IdHandler {
    public static async Task<MediatorResponse<InfiniLoreUser>> HandleAsync(
        // Message
        GetUserByAuth0IdQuery message,
        // Services
        IReadonlyUnitOfWorkFactory factory,
        // CT
        CancellationToken ct
    ) {
        if (message.Auth0Id.IsNullOrEmpty()) return MediatorResponse<InfiniLoreUser>.FromErrorString("Cannot get user id by auth0 id. Auth0 id is empty.");

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        Result<InfiniLoreUser> result = await userRepository.TryGetByAuth0IdAsync(message.Auth0Id, ct: ct);
        if (result.IsError) return MediatorResponse<InfiniLoreUser>.FromErrorString("Cannot get user id by auth0 id. Auth0 id not found.");

        return result.AsSuccess;
    }
}
