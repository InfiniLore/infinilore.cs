// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions;
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Collections.Immutable;
using System.Security.Claims;
using InfiniLoreClaimsStore = InfiniLore.Shared.Auth.InfiniLoreClaimsStore;

namespace InfiniLore.Server.Modules.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IAccessingUserProvider>]
public class AccessingUserProvider(
    IJwtTokenHelper jwtTokenHelper,
    IHttpContextAccessor httpContextAccessor
) : IAccessingUserProvider {
    public IAccessingUser Empty { get; } = new AccessingUser(
        Guid.Empty,
        ImmutableArray<string>.Empty,
        ImmutableArray<string>.Empty
    );
    public IAccessingUser Server { get; } = new AccessingUser(
        Guid.Empty,
        ImmutableArray<string>.Empty,
        ImmutableArray<string>.Empty
    );

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<IAccessingUser> FromJwtTokenAsync(CancellationToken ct = default) {
        if (jwtTokenHelper.IsNotAuthenticated) return AccessingUser.Empty;

        Guid userId = await jwtTokenHelper.TryGetUserIdFromClaimsAsync(ct);
        if (userId == Guid.Empty) return AccessingUser.Empty;

        return new AccessingUser(
            userId,
            jwtTokenHelper.GetRoles().ToImmutableArray(),
            jwtTokenHelper.GetPermissions().ToImmutableArray()
        );
    }

    public IAccessingUser FromClaims(CancellationToken ct = default) {
        ClaimsPrincipal? claims = httpContextAccessor.HttpContext?.User;
        
        string? userIdString = claims?.FindFirstOrDefault(InfiniLoreClaimsStore.UserId)?.Value;
        if (!Guid.TryParse(userIdString, out Guid userId)) return AccessingUser.Empty;

        ImmutableArray<string>? roles = claims?
            .FindAll(ClaimTypes.Role)
            .Select(claim => claim.Value)
            .ToImmutableArray();
        
        ImmutableArray<string>? permissions = claims?
            .FindAll("permissions")
            .Select(claim => claim.Value)
            .ToImmutableArray();

        ct.ThrowIfCancellationRequested();

        return new AccessingUser(
            userId,
            roles ?? ImmutableArray<string>.Empty,
            permissions ?? ImmutableArray<string>.Empty
        );
    }
}
