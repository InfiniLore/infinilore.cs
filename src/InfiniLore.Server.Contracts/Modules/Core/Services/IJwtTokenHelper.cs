// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace InfiniLore.Server.Modules.Core.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJwtTokenHelper {
    bool IsAuthenticated { get; }
    bool IsNotAuthenticated { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    bool TryGetUserId([NotNullWhen(true)] out string? userId);
    bool TryGetAuthenticatedUser([NotNullWhen(true)] out ClaimsPrincipal? user);
    bool TryGetRoles([NotNullWhen(true)] out string[]? roles);
    bool TryGetPermissions([NotNullWhen(true)] out string[]? permissions);

    Dictionary<string, List<string>> GetAllClaimsAsDictionary();
    ValueTask<Guid> TryGetUserIdFromClaimsAsync(CancellationToken ct = default);

    string[] GetRoles();
    string[] GetPermissions();
}
