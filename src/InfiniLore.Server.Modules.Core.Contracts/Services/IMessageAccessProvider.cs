// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.Modules.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMessageAccessProvider {
    IMessageAccess Empty { get; }
    IMessageAccess Server { get; }
    ValueTask<IMessageAccess> FromJwtTokenAsync(CancellationToken ct = default);
    IMessageAccess FromClaims(CancellationToken ct = default);
}
