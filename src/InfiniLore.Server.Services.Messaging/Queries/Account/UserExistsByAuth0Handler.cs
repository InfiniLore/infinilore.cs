// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database.Repositories.Account;

namespace InfiniLore.Server.Services.Messaging.Queries.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class UserExistsByAuth0Handler {
    public static async Task<MediatorResponse> HandleAsync(
        // Message
        UserExistsByAuth0Query message,
        // Services
        IReadonlyUnitOfWorkFactory factory,
        // CT
        CancellationToken ct
    ) {
        if (message.Auth0UserId.IsNullOrEmpty()) return false;

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        Result result = await userRepository.IsExistingAuth0Id(message.Auth0UserId, ct);
        if (!result.TryGetState(out bool state)) return result.AsError;

        return state;
    }
}
