// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace InfiniLore.Clients.Wasm.Services.AuthenticationStateSyncer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PersistentAuthenticationStateProvider(PersistentComponentState persistentState) : AuthenticationStateProvider {
    private static readonly Task<AuthenticationState> UnauthenticatedTask = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override Task<AuthenticationState> GetAuthenticationStateAsync() {
        if (!persistentState.TryTakeFromJson(nameof(UserInfo), out UserInfo? userInfo) || userInfo is null) return UnauthenticatedTask;

        Claim[] claims = [
            new(ClaimTypes.NameIdentifier, userInfo.UserId),
            new(ClaimTypes.Name, userInfo.Name),
            new(ClaimTypes.Email, userInfo.Email)
        ];

        var claimsIdentity = new ClaimsIdentity(
            claims,
            authenticationType: nameof(PersistentAuthenticationStateProvider)
        );

        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(claimsIdentity)));
    }
}