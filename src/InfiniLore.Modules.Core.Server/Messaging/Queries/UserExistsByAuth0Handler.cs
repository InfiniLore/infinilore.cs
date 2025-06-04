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
public class UserExistsByAuth0Handler(IReadonlyUnitOfWorkFactory factory) : CommandHandler<UserExistsByAuth0Query, Outcome> {
    public override async Task<Outcome> ExecuteAsync(UserExistsByAuth0Query command, CancellationToken ct = new()) {
        if (command.Auth0UserId.IsNullOrEmpty()) return Outcome.False;

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);

        Outcome result = await userRepository.IsExistingAuth0Id(command.Auth0UserId, ct);
        if (!result.TryGetAsState(out bool state)) return result.AsError;
        return Outcome.FromState(state);
    }
}
