// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace InfiniLore.ServerClient.Shared.JwtToken;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJwtTokenEncoder {
    bool TryDecodeJwt(string token, [NotNullWhen(true)] out string? payload);
    bool TryGetTokenUtcExpiry(string token, out DateTime expiry);
}
