// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Shared;
using JetBrains.Annotations;

namespace InfiniLore.Modules.Core.Server.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetUserIdByAuth0IdHandler(IReadonlyUnitOfWorkFactory factory) : CommandHandler<GetUserIdByAuth0IdQuery, Shared.Outcome<Guid>> {
    public override async Task<Shared.Outcome<Guid>> ExecuteAsync(GetUserIdByAuth0IdQuery command, CancellationToken ct = new()) {
        if (command.Auth0Id.IsNullOrEmpty()) return Shared.Outcome<Guid>.FromError("Cannot get user id by auth0 id. Auth0 id is empty.");

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);

        Outcome<Guid> result = await userRepository.TryGetIdByAuth0IdAsync(command.Auth0Id, ct);
        if (result.IsError) return Shared.Outcome<Guid>.FromError("Cannot get user id by auth0 id. Auth0 id not found.");

        return result.AsSuccess;
    }
}
