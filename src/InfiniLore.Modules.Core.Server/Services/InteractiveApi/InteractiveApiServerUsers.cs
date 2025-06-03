// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Modules.Core.Shared.Database;
using InfiniLore.Modules.Core.Shared.Extensions;
using Microsoft.AspNetCore.Components.Forms;

namespace InfiniLore.Modules.Core.Server.InteractiveApi;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInteractiveApiUsers>]
public class InteractiveApiServerUsers(IInteractiveApiServer interactiveApi) : IInteractiveApiUsers {
    private const int MaxFileSize = 5 * 1024 * 1024; // 5MB in bytes

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<AterraEngine.Unions.Result<IInfiniLoreUserModel>> GetUserAsync(string userId, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return AterraEngine.Unions.Result<IInfiniLoreUserModel>.FromError("Invalid userId");

        Outcome<InfiniLoreUserModel> outcome = await interactiveApi.MessageBroker.GetUserByIdAsync(parsedUserId, ct: ct);
        return outcome.Match(
            successCase: AterraEngine.Unions.Result<IInfiniLoreUserModel>.FromSuccess,
            errorCase: _ => AterraEngine.Unions.Result<IInfiniLoreUserModel>.FromError("Failed to retrieve user.")
        );
    }
    
    public async ValueTask<AterraEngine.Unions.Result> UpsertProfileImageAsync(string userId, IBrowserFile file, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return AterraEngine.Unions.Result.FromError("Invalid userId");
        
        await using MemoryStream stream = await file.ToMemoryStreamAsync(MaxFileSize, ct: ct);
        Outcome outcome = await interactiveApi.MessageBroker.UpsertUserProfileImageAsync(
            parsedUserId, 
            file.ContentType,
            stream,
            ct: ct
        );
        return outcome.Match<AterraEngine.Unions.Result>(
            stateCase: state => state,
            errorCase: _ => AterraEngine.Unions.Result.FromError("Failed to retrieve user.")
        );
    }
}
