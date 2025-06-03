// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace InfiniLore.Shared.Services.JwtToken;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJwtTokenEncoder {
    bool TryDecodeJwtHeader(string token, [NotNullWhen(true)] out string? header);
    bool TryDecodeJwtPayload(string token, [NotNullWhen(true)] out string? payload);
    bool TryGetJwtSignature(string token, [NotNullWhen(true)] out string? signature);

    bool TryGetTokenUtcExpiry(string token, out DateTime expiry);
}
