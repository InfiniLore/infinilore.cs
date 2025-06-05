// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.Core.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;

namespace InfiniLore.Modules.Core.Server.Auth;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<AuthenticationStateProvider>]
public class ServerAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider {
    private readonly IClaimsDtoHelper _claimsPrincipalHelper;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly PersistentComponentState _state;

    private readonly PersistingComponentStateSubscription _subscription;

    private Task<AuthenticationState>? _authenticationStateTask;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public ServerAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory scopeFactory,
        PersistentComponentState state,
        IClaimsDtoHelper claimsPrincipalHelper
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
        IClaimsDto auth0Info = _claimsPrincipalHelper.GetClaimsDto(authenticationState.User);
        if (!auth0Info.IsAuthenticated || auth0Info.IsEmpty) return;

        _state.PersistAsJson(nameof(IClaimsDto), auth0Info);
    }

    protected override void Dispose(bool disposing) {
        _subscription.Dispose();
        AuthenticationStateChanged -= OnAuthenticationStateChanged;
        base.Dispose(disposing);
    }
}
