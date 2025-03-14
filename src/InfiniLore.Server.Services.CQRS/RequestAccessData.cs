// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Services.Auth0;
using InfiniLore.Server.Contracts.Services.Cqrs;
using Newtonsoft.Json;

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
            UserId: userId,
            Roles: helper.GetRoles(),
            Permissions: helper.GetPermissions()
        );
    }
    
}

