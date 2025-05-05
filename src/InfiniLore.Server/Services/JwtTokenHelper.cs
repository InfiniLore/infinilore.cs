// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.Users.Messaging.Queries;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace InfiniLore.Server.Modules.Core.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IJwtTokenHelper>(ServiceLifetime.Scoped)]
public class JwtTokenHelper(IHttpContextAccessor httpContextAccessor) : IJwtTokenHelper {
    private readonly ClaimsPrincipal? _user = httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => _user?.Identity?.IsAuthenticated == true;
    public bool IsNotAuthenticated => _user?.Identity?.IsAuthenticated == false;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetUserId([NotNullWhen(true)] out string? userId) {
        userId = _user?.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId.IsNotNullOrWhiteSpace();
    }

    public bool TryGetAuthenticatedUser([NotNullWhen(true)] out ClaimsPrincipal? user) {
        if (_user?.Identity is not { IsAuthenticated: true }) {
            user = null;
            return false;
        }

        user = _user;
        return true;
    }

    public bool TryGetRoles([NotNullWhen(true)] out string[]? roles) {
        roles = GetRoles();
        return roles.Length > 0;
    }

    public bool TryGetPermissions([NotNullWhen(true)] out string[]? permissions) {
        permissions = GetPermissions();
        return permissions.Length > 0;
    }

    public Dictionary<string, List<string>> GetAllClaimsAsDictionary()
        => _user?.Claims
                .GroupBy(claim => claim.Type)
                .ToDictionary(keySelector: group => group.Key, elementSelector: group => group.Select(claim => claim.Value).ToList())
            ?? [];

    public async ValueTask<Guid> TryGetUserIdFromClaimsAsync(CancellationToken ct = default) {
        string? auth0UserId = _user?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? _user?.FindFirstValue("sub");

        if (auth0UserId.IsNullOrWhiteSpace()) return Guid.Empty;

        // TODO maybe not use mediator here? I dont know
        MessageResponse<Guid> result = await new GetUserIdByAuth0IdQuery(auth0UserId).ExecuteAsync(ct);
        if (!result.TryGetAsSuccess(out Guid userId)) return Guid.Empty;

        return userId != Guid.Empty
            ? userId
            : Guid.Empty;
    }

    public string[] GetRoles() => _user?.FindAll(ClaimTypes.Role).Select(claim => claim.Value).ToArray() ?? [];

    public string[] GetPermissions() => _user?.FindAll("permissions").Select(claim => claim.Value).ToArray() ?? [];
}
