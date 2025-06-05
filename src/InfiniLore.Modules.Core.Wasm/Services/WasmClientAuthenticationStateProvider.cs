// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace InfiniLore.Modules.Core.Wasm.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AuthenticationStateProviderWasm(
    PersistentComponentState persistentState,
    IClaimsDtoHelper principalHelper,
    ILogger<AuthenticationStateProviderWasm> logger
) : AuthenticationStateProvider {
    private static readonly Task<AuthenticationState> UnauthenticatedTask = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override Task<AuthenticationState> GetAuthenticationStateAsync() {
        if (!persistentState.TryTakeFromJson(nameof(IClaimsDto), out ClaimsDto? userInfo) || userInfo is null) {
            logger.Warning("No Auth0 information found in persistent state.");
            return UnauthenticatedTask;
        }

        ClaimsPrincipal principal = principalHelper.GetClaimsPrincipal<AuthenticationStateProviderWasm>(userInfo);
        logger.Information("Auth0 information found in persistent state.");
        return Task.FromResult(new AuthenticationState(principal));
    }
}
