// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database.Repositories.Account;

namespace InfiniLore.Server.Services.Mediator.Queries.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UsernameExistsHandler{
    public static async Task<MediatorResponse> HandleAsync(
        // Message
        UsernameExistsQuery message,
        // Services
        IReadonlyUnitOfWorkFactory factory,
        // CT
        CancellationToken ct
    ) {
        if (message.Username.IsNullOrWhiteSpace()) return MediatorResponse.FromErrorString("Cannot check for username existence. Username is empty.");

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        Result result = await userRepository.IsUsernameTakenAsync(message.Username, ct: ct);
        if (!result.TryGetState(out bool state)) return result.AsError;

        return state;
    }
}
