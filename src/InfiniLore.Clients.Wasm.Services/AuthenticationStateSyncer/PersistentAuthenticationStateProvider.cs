// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.ServerClient.Shared;
using InfiniLore.ServerClient.Shared.Auth0;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace InfiniLore.Clients.Wasm.Services.AuthenticationStateSyncer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PersistentAuthenticationStateProvider(PersistentComponentState persistentState, IAuthenticationStateProviderClaimsPrincipalHelper principalHelper, ILogger<PersistentAuthenticationStateProvider> logger) : AuthenticationStateProvider {
    private static readonly Task<AuthenticationState> UnauthenticatedTask = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override Task<AuthenticationState> GetAuthenticationStateAsync() {
        if (!persistentState.TryTakeFromJson(nameof(IAuth0Information), out Auth0Information? userInfo) || userInfo is null) {
            logger.Information("No Auth0 information found in persistent state.");
            return UnauthenticatedTask;
        }

        ClaimsPrincipal principal = principalHelper.GetClaimsPrincipal<PersistentAuthenticationStateProvider>(userInfo);
        logger.Information("Auth0 information found in persistent state.");
        return Task.FromResult(new AuthenticationState(principal));
    }
}
