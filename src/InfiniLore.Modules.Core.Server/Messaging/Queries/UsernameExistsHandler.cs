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
public class UsernameExistsHandler(IReadonlyUnitOfWorkFactory factory) : CommandHandler<UsernameExistsQuery, Outcome> {
    public override async Task<Outcome> ExecuteAsync(UsernameExistsQuery command, CancellationToken ct = new()) {
        if (command.Username.IsNullOrWhiteSpace()) return Outcome.FromError("Cannot check for username existence. Username is empty.");

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);

        Outcome result = await userRepository.IsUsernameTakenAsync(command.Username, ct: ct);
        if (!result.TryGetAsState(out bool state)) return result.AsError;

        return Outcome.FromState(state);
    }
}
