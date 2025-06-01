// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Modules.Core.Shared.Database;
using InfiniLore.Shared.Extensions;
using Microsoft.AspNetCore.Components.Forms;

namespace InfiniLore.Modules.Core.Server.InteractiveApi;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInteractiveApiUsers>]
public class InteractiveApiServerUsers(IInteractiveApiServer interactiveApi) : IInteractiveApiUsers {

    public async ValueTask<Result<IInfiniLoreUserModel>> GetUserAsync(string userId, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return Result<IInfiniLoreUserModel>.FromError("Invalid userId");
        
        MessageResponse<InfiniLoreUserModel> result = await interactiveApi.MessageBroker.GetUserByIdAsync(parsedUserId, ct: ct);
        return result.Match(
            successCase: Result<IInfiniLoreUserModel>.FromSuccess,
            errorCase: _ => Result<IInfiniLoreUserModel>.FromError("Failed to retrieve user.")
        );
    }
    
    public async ValueTask<Result> UpsertProfileImageAsync(string userId, IBrowserFile file, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return Result.FromError("Invalid userId");
        
        await using MemoryStream stream = await file.ToMemoryStreamAsync(ct: ct);
        MessageResponse result = await interactiveApi.MessageBroker.UpsertUserProfileImageAsync(
            parsedUserId, 
            file.ContentType,
            stream,
            ct: ct
        );
        return result.Match<Result>(
            stateCase: state => state,
            errorCase: _ => Result.FromError("Failed to retrieve user.")
        );
    }
}
