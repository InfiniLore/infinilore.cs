// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.ServerClient.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace InfiniLore.Clients.Wasm.Services.AuthenticationStateSyncer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PersistentAuthenticationStateProvider(PersistentComponentState persistentState, IAuthenticationStateProviderClaimsPrincipalHelper principalHelper) : AuthenticationStateProvider {
    private static readonly Task<AuthenticationState> UnauthenticatedTask = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override Task<AuthenticationState> GetAuthenticationStateAsync() {
        if (!persistentState.TryTakeFromJson(nameof(Auth0Information), out Auth0Information? userInfo) || userInfo is null) return UnauthenticatedTask;

        ClaimsPrincipal principal = principalHelper.GetClaimsPrincipal<PersistentAuthenticationStateProvider>(userInfo);
        return Task.FromResult(new AuthenticationState(principal));
    }
}
