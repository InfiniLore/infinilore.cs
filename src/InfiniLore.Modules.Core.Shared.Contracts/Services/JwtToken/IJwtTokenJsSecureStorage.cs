// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Shared.Services.JwtToken;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJwtTokenJsSecureStorage {
    Task SaveTokenAsync(string token, DateTime expiresAt, CancellationToken ct = default);
    Task<string?> GetTokenAsync(CancellationToken ct = default);
    Task RemoveTokenAsync(CancellationToken ct = default);
}
