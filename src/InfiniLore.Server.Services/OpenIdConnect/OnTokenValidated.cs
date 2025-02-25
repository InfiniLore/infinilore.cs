// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Contracts.Services.OpenIdConnectEventHelper;
using InfiniLore.Server.Services.CQRS.Queries.Account.Auth0;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InfiniLore.Server.Services.OpenIdConnect;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[InjectableService<OnTokenValidated>(ServiceLifetime.Scoped)]
public class OnTokenValidated(IMediator mediator, IOptions<IdentityOptions> options, ILoggerFactory loggerFactory) : IOpenIdConnectEventHelper<TokenValidatedContext> {
    private readonly ILogger _logger = loggerFactory.CreateLogger("Auth0 OnTokenValidated");

    private readonly IdentityOptions _options = options.Value;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task HandleAsync(TokenValidatedContext context) {
        if (context.Principal is not {} principal) {
            _logger.Debug("Principal not found, continuing...");
            return;
        }

        // Check if we find a userId within the claims
        //      This should always be present if the Token is validated, but you never know something might happen
        string? userId = principal.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;
        if (userId is null) {
            _logger.Debug("User ID not found, continuing...");
            return;
        }

        // Run all checks and return to new user page if needed
        if (await mediator.Send(new Auth0UserExistQuery(userId))) {
            _logger.Debug("User already exists, continuing...");
            return;
        }

        _logger.Information("User does not exist in ContentDb, redirecting to register new user...");

        string? returnUrl = context.Request.Query["returnUrl"];
        string encodedUserId = Uri.EscapeDataString(userId);
        string encodedReturnUrl = Uri.EscapeDataString(returnUrl ?? "/");

        context.Response.Redirect($"/account/register?auth0UserId={encodedUserId}&returnUrl={encodedReturnUrl}");
        context.HandleResponse();
    }
}
