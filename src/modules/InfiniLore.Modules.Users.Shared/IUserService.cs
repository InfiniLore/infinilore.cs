// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Users.Database;

namespace InfiniLore.Modules.Users;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IUserService {
    Task<UserModel?> GetCurrentUserAsync(CancellationToken ct = default);
    Task<bool> HasAnyUserAsync(CancellationToken ct = default);
    Task<UserModel?> GetOrCreateDefaultUserAsync(CancellationToken ct = default);
    Task SignInUserAsync(UserModel user, CancellationToken ct = default);
}
