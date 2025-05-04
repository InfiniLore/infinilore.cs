// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace InfiniLore.ServerClient.Shared.JwtToken;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IJwtTokenEncoder>(ServiceLifetime.Singleton)]
public class JwtTokenEncoder(ILogger<JwtTokenEncoder> logger) : IJwtTokenEncoder {
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------

    /// <summary>
    ///     Decodes the expiration date from the "exp" field in the payload.
    /// </summary>
    public bool TryGetTokenUtcExpiry(string token, out DateTime expiry) {
        expiry = DateTime.MinValue;
        try {
            string[] parts = token.Split('.');
            if (parts.Length != 3) return false;// Not a valid JWT

            // Decode payload
            string payloadJson = DecodeBase64(parts[1]);
            using JsonDocument document = JsonDocument.Parse(payloadJson);

            // Extract the "exp" field
            JsonElement root = document.RootElement;
            if (!root.TryGetProperty("exp", out JsonElement expClaim)) return false;// "exp" field not found

            long expSeconds = expClaim.GetInt64();
            // Convert from Unix timestamp to DateTime
            expiry = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
            return expiry != DateTime.MinValue;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to extract token expiry");
            return false;
        }
    }

    /// <summary>
    ///     Decodes the JWT header only.
    /// </summary>
    public bool TryDecodeJwtHeader(string token, [NotNullWhen(true)] out string? header) {
        header = null;
        try {
            string[] parts = token.Split('.');
            if (parts.Length != 3) return false;// Not a valid JWT

            string headerJson = DecodeBase64(parts[0]);
            JsonDocument headerDoc = JsonDocument.Parse(headerJson);
            header = JsonSerializer.Serialize(headerDoc.RootElement, Options);
            return true;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to decode JWT header");
            return false;
        }
    }

    /// <summary>
    ///     Decodes the JWT payload only.
    /// </summary>
    public bool TryDecodeJwtPayload(string token, [NotNullWhen(true)] out string? payload) {
        payload = null;
        try {
            string[] parts = token.Split('.');
            if (parts.Length != 3) return false;// Not a valid JWT

            string payloadJson = DecodeBase64(parts[1]);
            JsonDocument payloadDoc = JsonDocument.Parse(payloadJson);
            payload = JsonSerializer.Serialize(payloadDoc.RootElement, Options);
            return true;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to decode JWT payload");
            return false;
        }
    }

    /// <summary>
    ///     Fetches the raw signature from the JWT token.
    /// </summary>
    public bool TryGetJwtSignature(string token, [NotNullWhen(true)] out string? signature) {
        signature = null;
        try {
            string[] parts = token.Split('.');
            if (parts.Length != 3) return false;// Not a valid JWT

            signature = parts[2];
            return true;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to extract JWT signature");
            return false;
        }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------

    /// <summary>
    ///     Decodes a Base64-encoded string, padding if necessary.
    /// </summary>
    private static string DecodeBase64(string base64) {
        byte[] bytes = Convert.FromBase64String(PadBase64(base64));
        return Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    ///     Pads a Base64 string if it's not properly padded.
    /// </summary>
    private static string PadBase64(string base64)
        => (base64.Length % 4) switch {
            2 => $"{base64}==",
            3 => $"{base64}=",
            _ => base64
        };

    // -----------------------------------------------------------------------------------------------------------------
    // DTO for Decoded JWT
    // -----------------------------------------------------------------------------------------------------------------
}
