// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared.Database;

namespace InfiniLore.Modules.Core.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UserStateContainer(IJsRuntimeHelper jsRuntime) : IUserStateContainer {
    private IInfiniLoreUserModel? _savedUser;
    private bool? _isAuthenticated;
    
    public event Action? OnChange;

    private const string JsLocalStorageUser = "USERSTATE_User";
    private const string JsLocalStorageIsAuthenticated = "USERSTATE_IsAuthenticated";
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task SetUserAsync(IInfiniLoreUserModel user) {
        _savedUser = user as WasmInfiniLoreUserModel 
            ?? new WasmInfiniLoreUserModel(
                Id: user.Id,
                Username: user.Username,
                Auth0Id: null,
                ProfileImageUrl: user.ProfileImageUrl
            );
        await jsRuntime.StoreToLocalStorageAsync(JsLocalStorageUser, _savedUser);
        OnChange?.Invoke();
    }

    public async Task<IInfiniLoreUserModel?> GetUserAsync() {
        if (_savedUser is not null) return _savedUser;

        var possibleUser = await jsRuntime.GetFromLocalStorageAsync<WasmInfiniLoreUserModel>(JsLocalStorageUser);
        if (possibleUser is null) return null;

        _savedUser = possibleUser;
        return possibleUser;
    }
    
    public async Task RemoveUserAsync() {
        await jsRuntime.RemoveFromLocalStorageAsync(JsLocalStorageUser);
    }

    public async Task SetIsAuthenticatedAsync(bool isAuthenticated) {
        _isAuthenticated = isAuthenticated;
        await jsRuntime.StoreToLocalStorageAsync(JsLocalStorageIsAuthenticated, isAuthenticated);
        OnChange?.Invoke();
    }

    public async Task<bool> GetIsAuthenticatedAsync() {
        if (_isAuthenticated is not null) return (bool)_isAuthenticated;

        bool possibleAuthenticated = await jsRuntime.GetFromLocalStorageAsync<bool>(JsLocalStorageIsAuthenticated);
        _isAuthenticated = possibleAuthenticated;
        return possibleAuthenticated;
    }
}
