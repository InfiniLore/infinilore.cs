// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared.Database;

namespace InfiniLore.Modules.Core.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IUserStateContainer {
    Task SetUserAsync(IInfiniLoreUserModel user);
    Task<IInfiniLoreUserModel?> GetUserAsync();
    Task RemoveUserAsync();
    
    Task SetIsAuthenticatedAsync(bool isAuthenticated);
    Task<bool> GetIsAuthenticatedAsync();
    
    event Action? OnChange;
}
