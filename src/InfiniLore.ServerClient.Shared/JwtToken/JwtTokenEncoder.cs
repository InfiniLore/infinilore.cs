// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
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
    public bool TryDecodeJwt(string token,[NotNullWhen(true)] out string? payload) {
        payload = null;
        try {
            string[] parts = token.Split('.');
            if (parts.Length != 3) return false;// Not a valid JWT

            string encodedPayload = parts[1];
            byte[] json = Convert.FromBase64String(PadBase64(encodedPayload));
            payload = JsonSerializer.Serialize(JsonDocument.Parse(json).RootElement, Options);
            return true;
        }
        catch (Exception ex) {
            logger.Error(ex, "Failed to decode JWT");
            return false;
        }
    }

    public bool TryGetTokenUtcExpiry(string token, out DateTime expiry) {
        expiry = DateTime.MinValue;
        try {
            string[] parts = token.Split('.');
            if (parts.Length != 3) return false; // Not a valid JWT

            string payload = parts[1];
            byte[] json = Convert.FromBase64String(PadBase64(payload));

            using JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;

            // Extract the "exp" field
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


    private static string PadBase64(string base64)
        => (base64.Length % 4) switch {
            2 => $"{base64}==",
            3 => $"{base64}=",
            _ => base64
        };
}
