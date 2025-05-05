// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.ServerClient.Shared;
using Microsoft.AspNetCore.Http;
using System.Collections.Immutable;
using System.Security.Claims;

namespace InfiniLore.Server.Modules.Core.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IRequestDataFactory>]
public class RequestDataFactory(
    IJwtTokenHelper jwtTokenHelper,
    IHttpContextAccessor httpContextAccessor
) : IRequestDataFactory {
    
    public async ValueTask<IAccessData> FromJwtTokenAsync(CancellationToken ct = default) {
        if (jwtTokenHelper.IsNotAuthenticated) return RequestAccessData.Empty;

        Guid userId = await jwtTokenHelper.TryGetUserIdFromClaimsAsync(ct);
        if (userId == Guid.Empty) return RequestAccessData.Empty;

        return new RequestAccessData(
            userId,
            jwtTokenHelper.GetRoles().ToImmutableArray(),
            jwtTokenHelper.GetPermissions().ToImmutableArray()
        );
    }

    public IAccessData FromClaims(CancellationToken ct = default) {
        ClaimsPrincipal? claims = httpContextAccessor.HttpContext?.User;
        
        string? userIdString = claims?.FindFirstOrDefault(InfiniLoreClaimsStore.UserId)?.Value;
        if (!Guid.TryParse(userIdString, out Guid userId)) return RequestAccessData.Empty;

        ImmutableArray<string>? roles = claims?
            .FindAll(ClaimTypes.Role)
            .Select(claim => claim.Value)
            .ToImmutableArray();
        ImmutableArray<string>? permissions = claims?
            .FindAll("permissions")
            .Select(claim => claim.Value)
            .ToImmutableArray();

        ct.ThrowIfCancellationRequested();

        return new RequestAccessData(
            userId,
            roles ?? ImmutableArray<string>.Empty,
            permissions ?? ImmutableArray<string>.Empty
        );
    }
}
