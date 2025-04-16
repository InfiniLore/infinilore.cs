// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using JetBrains.Annotations;

namespace InfiniLore.Server.Services.Messaging.Queries.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetUserIdByAuth0IdHandler(IReadonlyUnitOfWorkFactory factory) : CommandHandler<GetUserIdByAuth0IdQuery, MessageResponse<Guid>> {
    public override async Task<MessageResponse<Guid>> ExecuteAsync(GetUserIdByAuth0IdQuery command, CancellationToken ct = new()) {
        if (command.Auth0Id.IsNullOrEmpty()) return MessageResponse<Guid>.FromErrorString("Cannot get user id by auth0 id. Auth0 id is empty.");

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        Result<Guid> result = await userRepository.TryGetIdByAuth0IdAsync(command.Auth0Id, ct);
        if (result.IsError) return MessageResponse<Guid>.FromErrorString("Cannot get user id by auth0 id. Auth0 id not found.");

        return result.AsSuccess;
    }
}
