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

    private readonly IdentityOptions _options = options.Value;
    private readonly ILogger _logger = loggerFactory.CreateLogger("Auth0 OnTokenValidated");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task HandleAsync(TokenValidatedContext context) {
        if (context.Principal is not {} principal) {
            _logger.Debug("Principal not found");
            return;
        }
        
        // Check if we find a userId within the claims
        //      This should always be present if the Token is validated, but you never know something might happen
        string? userId = principal.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;
        if (userId is null) {
            _logger.Debug("User ID not found");
            return;
        }
        
        // Run all checks and return to new user page if needed
        bool result = await mediator.Send(new Auth0UserExistQuery(userId) );
        if (result) {
            _logger.Debug("User already exists");
            return;
        }
        
        _logger.Warning("User does not exist, redirecting");
        
        string? returnUrl = context.Request.Query["returnUrl"];
        string encodedUserId = Uri.EscapeDataString(userId);
        string encodedReturnUrl = Uri.EscapeDataString(returnUrl ?? "/");
        
        context.Response.Redirect($"/account/register?auth0UserId={encodedUserId}&returnUrl={encodedReturnUrl}");
        context.HandleResponse();
    }
}
