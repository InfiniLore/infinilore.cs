// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Shared;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

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
    private const int DebounceMilliseconds = 500;
    private CancellationTokenSource? _debounceTokenSource;

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

        // Cancel any pending validation
        await (_debounceTokenSource?.CancelAsync() ?? Task.CompletedTask);
        _debounceTokenSource = new CancellationTokenSource();
        CancellationToken token = _debounceTokenSource.Token;

        // Show immediate feedback that validation is in progress
        _isFormDisabled = true;
        _usernameValidationMessage = "Checking...";
        StateHasChanged();

        try {
            await Task.Delay(DebounceMilliseconds, token);
            if (!token.IsCancellationRequested) await ValidateUsernameAsync(userModel.Username);
        }
        catch (OperationCanceledException) {
            // Ignore cancellation
        }
        finally {
            if (!token.IsCancellationRequested) StateHasChanged();
        }
    }

    private async Task ValidateUsernameAsync(string username) {
        if (username.IsNullOrWhiteSpace()) {
            _usernameValidationMessage = "Username cannot be empty.";
            _isFormDisabled = true;
            StateHasChanged();
            return;
        }

        // Call the backend to check username availability
        Outcome mediatorOutcome = await messageBroker.UsernameExistsAsync(username);

        if (!mediatorOutcome.TryGetAsState(out bool isTaken)) {
            _usernameValidationMessage = "Error checking username availability";
            _isFormDisabled = true;
            StateHasChanged();
            return;
        }

        _usernameValidationMessage = isTaken ? "Username is already taken" : string.Empty;
        _isFormDisabled = isTaken;
        StateHasChanged();
    }


    private async Task HandleValidSubmitAsync() {
        // Ensure username has passed asynchronous validation
        if (_isFormDisabled || !string.IsNullOrEmpty(_usernameValidationMessage)) return;

        Outcome<Guid> outcome = await messageBroker.CreateInfiniLoreUserAsync(Auth0UserId, userModel.Username);
        if (outcome.TryGetAsErrorValue(out string? errorMessage)) {
            _usernameValidationMessage = string.Join(", ", errorMessage);
            _isFormDisabled = false;// Allow retry
            StateHasChanged();
            return;
        }

        string returnUl = ReturnUrl.IsNotNullOrEmpty()
            ? ReturnUrl
            : "/";

        string encodedReturnUl = Uri.EscapeDataString(returnUl);
        navigation.NavigateTo($"account/login?redirectUri={encodedReturnUl}");
    }
}
