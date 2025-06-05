// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Modules.Core.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TokenEndpoint(IHttpContextAccessor httpContextAccessor, IJwtTokenEncoder tokenEncoder) : EndpointWithoutRequest<TokenResponse> {

    public override void Configure() {
        Get("/account/token");
        RoutePrefixOverride(RoutePrefixes.Empty);
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Handler
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task HandleAsync(CancellationToken ct) {
        if (httpContextAccessor.HttpContext is null || !httpContextAccessor.HttpContext.User.Identity!.IsAuthenticated)
            ThrowError("Unauthorized");

        string? accessToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        if (accessToken.IsNullOrEmpty()) ThrowError("Unauthorized");

        if (!tokenEncoder.TryGetTokenUtcExpiry(accessToken, out DateTime expiresAt)) ThrowError("Unauthorized");

        if (DateTime.UtcNow >= expiresAt) ThrowError("Unauthorized");

        Response = new TokenResponse {
            Token = accessToken,
            ExpiresAt = expiresAt.ToString("o")// ISO 8601 format for JS Date parsing
        };
    }
}
