// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Contracts.Services;
using InfiniLore.Server.Database.Models.Account;
using InfiniLore.Server.Services.Mediator;
using InfiniLore.Server.Services.Mediator.Queries.Account;
using InfiniLore.ServerClient.Shared;
using InfiniLore.ServerClient.Shared.ClaimsHelper;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace InfiniLore.Server.Services.OpenIdConnect;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[InjectableService<IOpenIdConnectEventHelper<TokenValidatedContext>>(ServiceLifetime.Scoped)]
public class OnTokenValidatedHandler(IMediator mediator, ILoggerFactory loggerFactory, IClaimsDtoHelper claimsPrincipalHelper) : IOpenIdConnectEventHelper<TokenValidatedContext> {
    private readonly ILogger _logger = loggerFactory.CreateLogger("AUTH0OPENID OnTokenValidated");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task HandleAsync(TokenValidatedContext context) {
        if (context.Principal is not {} principal) {
            _logger.Debug("Principal not found, continuing...");
            context.Response.Redirect("/account/logout?returnUrl=/");
            context.HandleResponse();
            return;
        }

        // Check if we find a userId within the claims
        //      This should always be present if the Token is validated, but you never know something might happen
        IClaimsDto auth0Info = claimsPrincipalHelper.GetClaimsDto(principal);
        if (!auth0Info.IsAuthenticated || auth0Info.IsEmpty) {
            _logger.Debug("Information could not be found in claims, continuing...");
            context.Response.Redirect("/account/logout?returnUrl=/");
            context.HandleResponse();
            return;
        }

        // Run all checks and return to new user page if needed
        Task<MediatorResponse> userExistsTask = mediator.Send(new UserExistsByAuth0Query(auth0Info.Auth0UserId));
        Task<MediatorResponse<InfiniLoreUser>> userTask = mediator.Send(new GetUserByAuth0IdQuery(auth0Info.Auth0UserId));

        (MediatorResponse userExistsResponse, MediatorResponse<InfiniLoreUser> userResponse) = await TaskWhenAllHelper.WhenAll(userExistsTask, userTask);

        switch (userExistsResponse, userResponse) {
            // User Exists and have a userId
            case ({ IsState: true, State: true }, { IsSuccess: true, AsSuccess: var user }): {
                _logger.Debug("User already exists, continuing...");

                var addedClaims = new ClaimsIdentity();
                if (principal.FindFirstOrDefault(InfiniLoreClaimsStoreConstants.UserId) is null) {
                    addedClaims.AddClaim(new Claim(InfiniLoreClaimsStoreConstants.UserId, user.Id.ToString()));
                }

                if (principal.FindFirstOrDefault(InfiniLoreClaimsStoreConstants.UserName) is null) {
                    addedClaims.AddClaim(new Claim(InfiniLoreClaimsStoreConstants.UserName, user.Username));
                }

                principal.AddIdentity(addedClaims);

                return;
            }

            // User does not exist, redirect to registration page
            case ({ IsState: true, State: true }, { IsSuccess: false }):
            case ({ IsState: true, State: false }, _): {
                RedirectToUserRegistration(context, auth0Info);
                return;
            }

            // Something else happend, which means an error
            default: {
                if (userExistsResponse.TryGetAsErrorValue(out ICollection<string>? failure)) {}
                else if (userResponse.TryGetAsErrorValue(out failure)) {}
                else { failure = new[] { "Unknown error" }; }

                RedirectToLogout(context, failure);
                return;

            }
        }
    }

    private void RedirectToLogout(TokenValidatedContext context, ICollection<string> failure) {
        _logger.LogError("Error checking if user exists : {@failure}", failure);
        context.Response.Redirect("/account/logout?returnUrl=/");
        context.HandleResponse();
    }

    private void RedirectToUserRegistration(TokenValidatedContext context, IClaimsDto auth0Info) {
        _logger.Information("User does not exist in ContentDb, redirecting to register new user...");

        string? returnUrl = context.Request.Query["returnUrl"];
        string encodedUserId = Uri.EscapeDataString(auth0Info.Auth0UserId);
        string encodedReturnUrl = Uri.EscapeDataString(returnUrl ?? "/");

        context.Response.Redirect($"/account/register?auth0UserId={encodedUserId}&returnUrl={encodedReturnUrl}");
        context.HandleResponse();
    }
}
