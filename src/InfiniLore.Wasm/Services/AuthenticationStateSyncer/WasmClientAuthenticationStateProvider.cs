// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Shared.ClaimsHelper;
using InfiniLore.Shared.Services.ClaimsHelper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace InfiniLore.Wasm.Services.AuthenticationStateSyncer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class WasmClientAuthenticationStateProvider(
    PersistentComponentState persistentState,
    IClaimsDtoHelper principalHelper,
    ILogger<WasmClientAuthenticationStateProvider> logger
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

        ClaimsPrincipal principal = principalHelper.GetClaimsPrincipal<WasmClientAuthenticationStateProvider>(userInfo);
        logger.Information("Auth0 information found in persistent state.");
        return Task.FromResult(new AuthenticationState(principal));
    }
}
