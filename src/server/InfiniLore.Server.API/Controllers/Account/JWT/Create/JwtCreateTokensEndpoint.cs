// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Services;
using InfiniLore.Server.Data.Models.Account;
using InfiniLoreLib.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ProblemDetails=FastEndpoints.ProblemDetails;

namespace InfiniLore.Server.API.Controllers.Account.JWT.Create;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class JwtCreateTokensEndpoint(IApiSignInService apiSignInService, IJwtTokenService jwtTokenService, ILogger logger)
    : Endpoint<
        JwtCreateTokensRequest,
        Results<
            BadRequest<ProblemDetails>,
            Ok<JwtResponse>
        >
    > {
    public override void Configure() {
        Post("/account/jwt/token/create");
        AllowAnonymous();
    }

    public override async Task<Results<BadRequest<ProblemDetails>, Ok<JwtResponse>>> ExecuteAsync(JwtCreateTokensRequest req, CancellationToken ct) {
        try {
            IdentityUserResult<InfiniLoreUser> signInResult = await apiSignInService.SignInAsync(req.Username, req.Password, ct).ConfigureAwait(false);
            if (signInResult is not { IsSuccess: true, User: {} user }) {
                logger.Warning("Sign-in failed for user {Username}. Error: {ErrorMessage}", req.Username, signInResult.ErrorMessage);
                return TypedResults.BadRequest(new ProblemDetails { Detail = signInResult.ErrorMessage });
            }

            JwtResult jwtResult = await jwtTokenService.GenerateTokensAsync(user, req.Roles, req.Permissions, req.RefreshExpiresInDays, ct).ConfigureAwait(false);
            if (jwtResult is {
                    IsSuccess: true,
                    AccessTokenExpiryUtc: {} accessTokenExpiryUtc,
                    RefreshToken: {} refreshToken,
                    RefreshTokenExpiryUtc: {} refreshTokenExpiryUtc,
                    AccessToken: {} accessToken
                }) {
                logger.Information("JWT tokens generated successfully for user {Username}.", req.Username);
                return TypedResults.Ok(new JwtResponse(accessToken, accessTokenExpiryUtc, refreshToken, refreshTokenExpiryUtc));
            }

            logger.Warning("Token generation failed for user {Username}. Error: {ErrorMessage}", req.Username, jwtResult.ErrorMessage);
            return TypedResults.BadRequest(new ProblemDetails { Detail = "Unable to generate tokens." });

        }
        catch (Exception ex) {
            logger.Error(ex, "An unexpected error occurred while processing JWT token creation for user {Username}", req.Username);
            return TypedResults.BadRequest(new ProblemDetails { Detail = "An unexpected error occurred." });
        }
    }
}
