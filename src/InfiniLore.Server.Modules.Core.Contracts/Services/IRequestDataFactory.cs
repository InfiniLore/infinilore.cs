// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Security.Claims;

namespace InfiniLore.Server.Modules.Core.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IRequestDataFactory {
    ValueTask<IAccessData> FromJwtTokenAsync(CancellationToken ct = default);
    IAccessData FromClaims(CancellationToken ct = default);
}
