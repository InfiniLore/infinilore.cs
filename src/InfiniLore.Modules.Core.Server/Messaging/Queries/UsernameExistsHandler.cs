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
public class UsernameExistsHandler(IReadonlyUnitOfWorkFactory factory) : CommandHandler<UsernameExistsQuery, MessageResponse> {
    public override async Task<MessageResponse> ExecuteAsync(UsernameExistsQuery command, CancellationToken ct = new()) {
        if (command.Username.IsNullOrWhiteSpace()) return MessageResponse.FromErrorString("Cannot check for username existence. Username is empty.");

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);

        Result result = await userRepository.IsUsernameTakenAsync(command.Username, ct: ct);
        if (!result.TryGetState(out bool? state)) return result.AsError;

        return state;
    }
}
