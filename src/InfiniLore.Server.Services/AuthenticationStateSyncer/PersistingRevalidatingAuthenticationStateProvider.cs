// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Security.Claims;

namespace InfiniLore.Server.Services.AuthenticationStateSyncer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PersistingRevalidatingAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider {
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly PersistentComponentState _state;
    private readonly IdentityOptions _options;

    private readonly PersistingComponentStateSubscription _subscription;

    private Task<AuthenticationState>? _authenticationStateTask;

    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public PersistingRevalidatingAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory scopeFactory,
        PersistentComponentState state,
        IOptions<IdentityOptions> options
    )
        : base(loggerFactory) {
        _scopeFactory = scopeFactory;
        _state = state;
        _options = options.Value;

        AuthenticationStateChanged += OnAuthenticationStateChanged;
        _subscription = state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState authenticationState, CancellationToken cancellationToken
    ) {
        // Get the user manager from a new scope to ensure it fetches fresh data
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
        return ValidateSecurityStampAsync(authenticationState.User);
    }

    private static bool ValidateSecurityStampAsync(ClaimsPrincipal principal) => principal.Identity?.IsAuthenticated is not false;
    private void OnAuthenticationStateChanged(Task<AuthenticationState> authenticationStateTask) => _authenticationStateTask = authenticationStateTask;

    private async Task OnPersistingAsync() {
        if (_authenticationStateTask is null) {
            throw new UnreachableException($"Authentication state not set in {nameof(RevalidatingServerAuthenticationStateProvider)}.{nameof(OnPersistingAsync)}().");
        }

        AuthenticationState authenticationState = await _authenticationStateTask;
        ClaimsPrincipal principal = authenticationState.User;

        if (principal.Identity?.IsAuthenticated != true) return;

        string? userId = principal.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;
        string? name = principal.FindFirst("name")?.Value;
        string? email = principal.FindFirst("email")?.Value;

        if (userId is null || name is null || email is null) return;

        _state.PersistAsJson(nameof(UserInfo), new UserInfo(userId, name, email));
    }

    protected override void Dispose(bool disposing) {
        _subscription.Dispose();
        AuthenticationStateChanged -= OnAuthenticationStateChanged;
        base.Dispose(disposing);
    }
}
