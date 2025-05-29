// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.Modules.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAccessingUserProvider {
    IAccessingUser GetServerUser();
    ValueTask<IAccessingUser> FromJwtTokenAsync(CancellationToken ct = default);
    IAccessingUser FromClaims(CancellationToken ct = default);
}
