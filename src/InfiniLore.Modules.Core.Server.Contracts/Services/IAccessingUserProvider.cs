// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAccessingUserProvider {
    IAccessingUser FromServer();
    ValueTask<IAccessingUser> FromJwtTokenAsync(CancellationToken ct = default);
    IAccessingUser FromClaims(CancellationToken ct = default);
}
