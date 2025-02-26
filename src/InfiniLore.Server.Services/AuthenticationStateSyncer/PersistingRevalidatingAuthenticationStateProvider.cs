// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Contracts.Services.ClaimsPrincipalHelper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;

namespace InfiniLore.Server.Services.AuthenticationStateSyncer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<AuthenticationStateProvider>(ServiceLifetime.Scoped)]
public class PersistingRevalidatingAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider {
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly PersistentComponentState _state;
    private readonly IClaimsPrincipalHelper _claimsPrincipalHelper;

    private readonly PersistingComponentStateSubscription _subscription;

    private Task<AuthenticationState>? _authenticationStateTask;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public PersistingRevalidatingAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory scopeFactory,
        PersistentComponentState state,
        IClaimsPrincipalHelper claimsPrincipalHelper
    ) : base(loggerFactory) {
        _scopeFactory = scopeFactory;
        _state = state;
        _claimsPrincipalHelper = claimsPrincipalHelper;

        AuthenticationStateChanged += OnAuthenticationStateChanged;
        _subscription = state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
    }

    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

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
        IAuth0Information auth0Info = _claimsPrincipalHelper.GetAuth0Information(authenticationState.User);
        if (!auth0Info.IsAuthenticated || auth0Info.IsEmpty) return;

        _state.PersistAsJson(nameof(IAuth0Information), auth0Info);
    }

    protected override void Dispose(bool disposing) {
        _subscription.Dispose();
        AuthenticationStateChanged -= OnAuthenticationStateChanged;
        base.Dispose(disposing);
    }
}
