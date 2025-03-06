// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Contracts.Services;
using InfiniLore.Server.Services.CQRS.Queries.Account;
using InfiniLore.ServerClient.Shared;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.OpenIdConnect;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[InjectableService<IOpenIdConnectEventHelper<TokenValidatedContext>>(ServiceLifetime.Scoped)]
public class OnTokenValidated(IMediator mediator, ILoggerFactory loggerFactory, IAuthenticationStateProviderClaimsPrincipalHelper claimsPrincipalHelper ) : IOpenIdConnectEventHelper<TokenValidatedContext> {
    private readonly ILogger _logger = loggerFactory.CreateLogger("AUTH0OPENID OnTokenValidated");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask HandleAsync(TokenValidatedContext context) {
        if (context.Principal is not {} principal) {
            _logger.Debug("Principal not found, continuing...");
            return;
        }

        // Check if we find a userId within the claims
        //      This should always be present if the Token is validated, but you never know something might happen
        IAuth0Information auth0Info = claimsPrincipalHelper.GetAuth0Information(principal);
        if (!auth0Info.IsAuthenticated || auth0Info.IsEmpty) {
            _logger.Debug("Information could not be found in claims, continuing...");
            context.Response.Redirect("/account/logout?returnUrl=/");
            return;
        }

        // Run all checks and return to new user page if needed
        switch (await mediator.Send(new UserExistsByAuth0Query(auth0Info.UserId))) {
            case { IsState: true, State: true }: {
                _logger.Debug("User already exists, continuing...");
                return;
            }
            case { IsError: true, AsError.Value: {} failure }: {
                _logger.LogError("Error checking if user exists : {failure}", failure);
                context.Response.Redirect("/account/logout?returnUrl=/");
                return;
            }
        }

        _logger.Information("User does not exist in ContentDb, redirecting to register new user...");

        string? returnUrl = context.Request.Query["returnUrl"];
        string encodedUserId = Uri.EscapeDataString(auth0Info.UserId);
        string encodedReturnUrl = Uri.EscapeDataString(returnUrl ?? "/");

        context.Response.Redirect($"/account/register?auth0UserId={encodedUserId}&returnUrl={encodedReturnUrl}");
        context.HandleResponse();
    }
}
