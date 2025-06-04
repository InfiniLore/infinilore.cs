// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJsSecureStorage {
    Task SaveTokenAsync(string storageKey, string token, DateTime expiresAt, CancellationToken ct = default);
    Task<T?> GetTokenAsync<T>(string storageKey, CancellationToken ct = default);
    Task RemoveTokenAsync(string storageKey, CancellationToken ct = default);
}
