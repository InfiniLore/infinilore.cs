// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions;
using InfiniLore.Server.Contracts.Services.Auth0;
using InfiniLore.Server.Contracts.Services.Cqrs;
using InfiniLore.ServerClient.Shared;
using Newtonsoft.Json;
using System.Security.Claims;

namespace InfiniLore.Server.Services.CQRS;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record RequestAccessData(
    [JsonProperty("user_id")] Guid UserId,
    [JsonProperty("roles")] string[] Roles,
    [JsonProperty("permissions")] string[] Permissions
) : IAccessData {
    public static readonly IAccessData Empty = new RequestAccessData(Guid.Empty, [], []);

    public static async ValueTask<IAccessData> FromJwtTokenAsync(IJwtTokenHelper helper, CancellationToken ct = default) {
        if (helper.IsNotAuthenticated) return Empty;

        Guid userId = await helper.TryGetUserIdFromClaimsAsync(ct);
        if (userId == Guid.Empty) return Empty;

        return new RequestAccessData(
            userId,
            helper.GetRoles(),
            helper.GetPermissions()
        );
    }

    public static IAccessData FromClaims(ClaimsPrincipal? claimsPrincipal, CancellationToken ct = default) {

        string? userIdString = claimsPrincipal?.FindFirstOrDefault(InfiniLoreClaimsStore.UserId)?.Value;
        if (!Guid.TryParse(userIdString, out Guid userId)) return Empty;

        string[] roles = claimsPrincipal?.FindAll(ClaimTypes.Role).Select(claim => claim.Value).ToArray() ?? [];
        string[] permissions = claimsPrincipal?.FindAll("permissions").Select(claim => claim.Value).ToArray() ?? [];

        ct.ThrowIfCancellationRequested();

        return new RequestAccessData(
            userId,
            roles,
            permissions
        );
    }
}
