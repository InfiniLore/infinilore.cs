// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InfiniLore.ServerClient.Shared.JwtToken;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<JwtTokenEncoder>(ServiceLifetime.Singleton)]
public class JwtTokenEncoder(ILogger<JwtTokenEncoder> logger) {
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };
    public string? DecodeJwt(string token) {
        try {
            string[] parts = token.Split('.');
            if (parts.Length != 3) return null;// Not a valid JWT

            string payload = parts[1];
            byte[] json = Convert.FromBase64String(PadBase64(payload));
            return JsonSerializer.Serialize(JsonDocument.Parse(json).RootElement, Options);
        }
        catch (Exception ex) {
            logger.Error(ex, "Failed to decode JWT");
            return null;
        }
    }

    public DateTime GetTokenExpiry(string token) {
        try {
            string[] parts = token.Split('.');
            if (parts.Length != 3) return DateTime.MinValue; // Not a valid JWT

            string payload = parts[1];
            byte[] json = Convert.FromBase64String(PadBase64(payload));

            using JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;

            // Extract the "exp" field
            if (!root.TryGetProperty("exp", out JsonElement expClaim)) return DateTime.MinValue;// "exp" field not found

            long expSeconds = expClaim.GetInt64();
            // Convert from Unix timestamp to DateTime
            return DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;

        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to extract token expiry");
            return DateTime.MinValue;
        }
    }


    private static string PadBase64(string base64)
        => (base64.Length % 4) switch {
            2 => $"{base64}==",
            3 => $"{base64}=",
            _ => base64
        };
}
