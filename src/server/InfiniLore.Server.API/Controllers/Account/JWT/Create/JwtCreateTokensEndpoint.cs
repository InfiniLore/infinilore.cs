// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Services;
using InfiniLore.Server.Data.Models.Account;
using InfiniLoreLib.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using ProblemDetails=FastEndpoints.ProblemDetails;
using SignInResult=Microsoft.AspNetCore.Identity.SignInResult;

namespace InfiniLore.Server.API.Controllers.Account.JWT.Create;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class JwtCreateTokensEndpoint(SignInManager<InfiniLoreUser> signInManager, IJwtTokenService jwtTokenService, ILogger logger)
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

    public async override Task<Results<BadRequest<ProblemDetails>, Ok<JwtResponse>>> ExecuteAsync(JwtCreateTokensRequest req, CancellationToken ct) {
        logger.Information("Processing JWT token creation for user {@Username}", req.Username);

        if (await signInManager.UserManager.FindByNameAsync(req.Username) is not {} user ) {
            logger.Warning("User {@Username} not found", req.Username);
            return TypedResults.BadRequest(new ProblemDetails { Detail = "Invalid username" });
        }

        if (!await signInManager.CanSignInAsync(user)) {
            logger.Warning("User {@Username} cannot sign in", req.Username);
            return TypedResults.BadRequest(new ProblemDetails { Detail = "Unable to sign in." });
        }

        SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, req.Password, false);
        if (!signInResult.Succeeded) {
            logger.Warning("Invalid password for user {@Username}", req.Username);
            return TypedResults.BadRequest(new ProblemDetails { Detail = "Invalid username or password." });
        }

        logger.Information("Generating tokens for user {@Username}", req.Username);
        JwtResult jwtResult = await jwtTokenService.GenerateTokensAsync(user, req.Roles, req.Permissions, req.RefreshExpiresInDays,  ct);

        if (!jwtResult.IsFailure) {
            logger.Information("Tokens generated successfully for user {@Username}", req.Username);
            return TypedResults.Ok(new JwtResponse(
                jwtResult.AccessToken!,
                (DateTime)jwtResult.AccessTokenExpiryUtc!,
                (Guid)jwtResult.RefreshToken!,
                (DateTime)jwtResult.RefreshTokenExpiryUtc!
            ));
        }

        logger.Warning("Unable to generate tokens for user {@Username}. Result: {@JwtResult}", req.Username, jwtResult.ErrorMessage);
        return TypedResults.BadRequest(new ProblemDetails { Detail = "Unable to generate tokens." });

    }
}
