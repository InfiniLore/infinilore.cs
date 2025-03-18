// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.ServerClient.Shared.JwtToken;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJwtTokenProvider {
    Task SaveTokenAsync(string token, DateTime expiresAt, CancellationToken ct = default);
    Task<string?> GetTokenAsync(CancellationToken ct = default);
    Task RemoveTokenAsync(CancellationToken ct = default);
}
