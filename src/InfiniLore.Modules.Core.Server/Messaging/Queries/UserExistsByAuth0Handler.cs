// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Modules.Core.Server.Database;
using JetBrains.Annotations;

namespace InfiniLore.Modules.Core.Server.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class UserExistsByAuth0Handler(IReadonlyUnitOfWorkFactory factory) : CommandHandler<UserExistsByAuth0Query, MessageResponse> {
    public override async Task<MessageResponse> ExecuteAsync(UserExistsByAuth0Query command, CancellationToken ct = new()) {
        if (command.Auth0UserId.IsNullOrEmpty()) return false;

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);

        Result result = await userRepository.IsExistingAuth0Id(command.Auth0UserId, ct);
        if (!result.TryGetState(out bool? state)) return result.AsError;

        return state;
    }
}
