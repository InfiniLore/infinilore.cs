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
[InjectableScoped<IMessageAccessProvider>]
public class MessageAccessProvider(
    IJwtTokenHelper jwtTokenHelper,
    IHttpContextAccessor httpContextAccessor
) : IMessageAccessProvider {
    public IMessageAccess Empty => MessageAccess.Empty;
    public IMessageAccess Server => MessageAccess.Server;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<IMessageAccess> FromJwtTokenAsync(CancellationToken ct = default) {
        if (jwtTokenHelper.IsNotAuthenticated) return MessageAccess.Empty;

        Guid userId = await jwtTokenHelper.TryGetUserIdFromClaimsAsync(ct);
        if (userId == Guid.Empty) return MessageAccess.Empty;

        return new MessageAccess(
            userId,
            jwtTokenHelper.GetRoles().ToImmutableArray(),
            jwtTokenHelper.GetPermissions().ToImmutableArray()
        );
    }

    public IMessageAccess FromClaims(CancellationToken ct = default) {
        ClaimsPrincipal? claims = httpContextAccessor.HttpContext?.User;
        
        string? userIdString = claims?.FindFirstOrDefault(InfiniLoreClaimsStore.UserId)?.Value;
        if (!Guid.TryParse(userIdString, out Guid userId)) return MessageAccess.Empty;

        ImmutableArray<string>? roles = claims?
            .FindAll(ClaimTypes.Role)
            .Select(claim => claim.Value)
            .ToImmutableArray();
        
        ImmutableArray<string>? permissions = claims?
            .FindAll("permissions")
            .Select(claim => claim.Value)
            .ToImmutableArray();

        ct.ThrowIfCancellationRequested();

        return new MessageAccess(
            userId,
            roles ?? ImmutableArray<string>.Empty,
            permissions ?? ImmutableArray<string>.Empty
        );
    }
}
