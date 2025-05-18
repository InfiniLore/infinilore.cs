// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Server.Modules.Users.Database;
using InfiniLore.Server.Modules.Users.Messaging.Queries;
using JetBrains.Annotations;

namespace InfiniLore.Server.Modules.Core.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetUserByAuth0IdHandler(IReadonlyUnitOfWorkFactory factory) : CommandHandler<GetUserByAuth0IdQuery, MessageResponse<InfiniLoreUserModel>> {
    public override async Task<MessageResponse<InfiniLoreUserModel>> ExecuteAsync(GetUserByAuth0IdQuery command, CancellationToken ct = new()) {
        if (command.Auth0Id.IsNullOrEmpty()) return MessageResponse<InfiniLoreUserModel>.FromErrorString("Cannot get user id by auth0 id. Auth0 id is empty.");

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);

        Result<InfiniLoreUserModel> result = await userRepository.TryGetByAuth0IdAsync(command.Auth0Id, ct: ct);
        
        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (result.IsError) return MessageResponse<InfiniLoreUserModel>.FromErrorString("Cannot get user id by auth0 id. Auth0 id not found.");
        return MessageResponse<InfiniLoreUserModel>.FromSuccess(result.AsSuccess);
    }
}
