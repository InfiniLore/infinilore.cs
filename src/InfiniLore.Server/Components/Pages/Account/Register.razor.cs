// -----------------------------------------------------------------------------------------------------------------
// Methods
// -----------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Messaging;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Result=InfiniLore.Modules.Core.Server.Result;

namespace InfiniLore.Server.Components.Pages.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public partial class Register(
    NavigationManager navigation,
    [FromKeyedServices(IMessageBroker.FromServer)] IMessageBroker messageBroker
) {
    [Parameter] [SupplyParameterFromQuery(Name = "auth0UserId")] public string Auth0UserId { get; set; } = string.Empty;
    [Parameter] [SupplyParameterFromQuery(Name = "returnUrl")] public string ReturnUrl { get; set; } = string.Empty;

    private readonly UserModel userModel = new();

    private string _usernameValidationMessage = string.Empty;
    private bool _isFormDisabled;
    private readonly Stopwatch _debounceStopwatch = new();
    private const int DebounceMilliseconds = 500;
    private UsernameStatus _usernameStatus = UsernameStatus.None;

    private class UserModel {
        [Required]
        [StringLength(50, ErrorMessage = "Username must be less than 50 characters.")]
        public string Username { get; set; } = string.Empty;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private async Task OnUsernameInputAsync(ChangeEventArgs e) {
        userModel.Username = e.Value?.ToString() ?? string.Empty;

        if (!_debounceStopwatch.IsRunning) {
            _debounceStopwatch.Start();
        }
        else {
            _debounceStopwatch.Restart();
        }

        // Correctly Set Status to "Checking"
        _usernameValidationMessage = string.Empty;
        _usernameStatus = UsernameStatus.Checking;
        _isFormDisabled = true;
        await InvokeAsync(StateHasChanged);// Trigger UI Refresh

        // Debounced validation logic
        await Task.Delay(DebounceMilliseconds);
        if (_debounceStopwatch.ElapsedMilliseconds >= DebounceMilliseconds) {
            _debounceStopwatch.Stop();
            await ValidateUsernameAsync(userModel.Username);
        }
    }
    private async Task ValidateUsernameAsync(string username) {
        if (username.IsNullOrWhiteSpace()) {
            _usernameValidationMessage = "Username cannot be empty.";
            _isFormDisabled = true;
            _usernameStatus = UsernameStatus.None;// Set to None for an empty username
            await InvokeAsync(StateHasChanged);// Refresh UI
            return;
        }

        // Call the backend to check username availability
        Result mediatorResult = await messageBroker.UsernameExistsAsync(username);

        if (!mediatorResult.TryGetState(out bool? isTaken)) {
            _usernameStatus = UsernameStatus.None;// Default/Fallback if the response doesn't return properly
            _isFormDisabled = true;
            await InvokeAsync(StateHasChanged);// Refresh UI
            return;
        }

        // Update the UI based on validation response
        _usernameStatus = isTaken ?? false
            ? UsernameStatus.Taken
            : UsernameStatus.Available;

        _usernameValidationMessage = string.Empty;// Clear message
        _isFormDisabled = isTaken ?? false;// Disable form if a username is taken
        await InvokeAsync(StateHasChanged);// Refresh UI
    }
    
    private async Task HandleValidSubmitAsync() {
        // Ensure username has passed asynchronous validation
        if (_isFormDisabled || !string.IsNullOrEmpty(_usernameValidationMessage)) return;

        InfiniLore.Modules.Core.Server.Result<Guid> result = await messageBroker.CreateInfiniLoreUserAsync(Auth0UserId, userModel.Username);
        if (result.TryGetAsError(out Error<ICollection<string>>? errorMessage)) {
            _usernameValidationMessage = string.Join(", ", errorMessage.Value);
            _isFormDisabled = false; // Allow retry
            await InvokeAsync(StateHasChanged);
            return;
        }

        string returnUl = ReturnUrl.IsNotNullOrEmpty()
            ? ReturnUrl
            : "/";

        string encodedReturnUl = Uri.EscapeDataString(returnUl);
        navigation.NavigateTo($"Account/Login?redirectUri={encodedReturnUl}");
    }

    private enum UsernameStatus {
        None,
        Checking,
        Taken,
        Available
    }
}
