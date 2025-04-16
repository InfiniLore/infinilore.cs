// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Database.Models.Account;
using JetBrains.Annotations;

namespace InfiniLore.Server.Services.Messaging.Queries.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetUserByAuth0IdHandler(IReadonlyUnitOfWorkFactory factory) : CommandHandler<GetUserByAuth0IdQuery, MessageResponse<InfiniLoreUser>> {
    public override async Task<MessageResponse<InfiniLoreUser>> ExecuteAsync(GetUserByAuth0IdQuery command, CancellationToken ct = new()) {
        if (command.Auth0Id.IsNullOrEmpty()) return MessageResponse<InfiniLoreUser>.FromErrorString("Cannot get user id by auth0 id. Auth0 id is empty.");

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        Result<InfiniLoreUser> result = await userRepository.TryGetByAuth0IdAsync(command.Auth0Id, ct: ct);
        if (result.IsError) return MessageResponse<InfiniLoreUser>.FromErrorString("Cannot get user id by auth0 id. Auth0 id not found.");

        return result.AsSuccess;
    }
}
