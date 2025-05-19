// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.Modules.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMessageAccessFactory {
    IMessageAccess Empty { get; }
    ValueTask<IMessageAccess> FromJwtTokenAsync(CancellationToken ct = default);
    IMessageAccess FromClaims(CancellationToken ct = default);
}
