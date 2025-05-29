// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.Modules.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAccessingUserProvider {
    IAccessingUser Empty { get; }
    IAccessingUser Server { get; }
    ValueTask<IAccessingUser> FromJwtTokenAsync(CancellationToken ct = default);
    IAccessingUser FromClaims(CancellationToken ct = default);
}
