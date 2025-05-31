// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Modules.Core.Shared.Database;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Core.Server.InteractiveApi;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IUserInteractiveApi>]
public class UserInteractiveApiServer(
    [FromKeyedServices(IMessageBroker.FromClaims)] IMessageBroker messageBroker
) : IUserInteractiveApi {

    public async ValueTask<Result<IInfiniLoreUserModel>> GetUserAsync(string userId, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return Result<IInfiniLoreUserModel>.FromError("Invalid userId");
        
        MessageResponse<InfiniLoreUserModel> result = await messageBroker.GetUserByIdAsync(parsedUserId, ct: ct);
        return result.Match(
            successCase: Result<IInfiniLoreUserModel>.FromSuccess,
            errorCase: _ => Result<IInfiniLoreUserModel>.FromError("Failed to retrieve user.")
        );
    }
}
