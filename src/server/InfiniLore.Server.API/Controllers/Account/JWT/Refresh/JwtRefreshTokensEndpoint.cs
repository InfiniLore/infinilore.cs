// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Services;
using InfiniLoreLib.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InfiniLore.Server.API.Controllers.Account.JWT.Refresh;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class JwtRefreshTokensEndpoint(IJwtTokenService jwtTokenService, ILogger logger) 
    : Endpoint<
        JwtRefreshTokensRequest,
        Results<
            BadRequest<ProblemDetails>,
            Ok<JwtResponse>
        >
    > {
    public override void Configure() {
        Post("/account/jwt/token/refresh");
        AllowAnonymous();
    }
    public async override Task<Results<BadRequest<ProblemDetails>, Ok<JwtResponse>>> ExecuteAsync(JwtRefreshTokensRequest req, CancellationToken ct) {
        logger.Information("Generating tokens for refreshToken {@Token}", req.RefreshToken);
        JwtResult jwtResult = await jwtTokenService.RefreshTokensAsync(req.RefreshToken, ct);

        if (!jwtResult.IsFailure) {
            logger.Information("Tokens generated successfully for refreshToken {@Token}", req.RefreshToken);
            return TypedResults.Ok(new JwtResponse(
                jwtResult.AccessToken!,
                (DateTime)jwtResult.AccessTokenExpiryUtc!,
                (Guid)jwtResult.RefreshToken!,
                (DateTime)jwtResult.RefreshTokenExpiryUtc!
            ));
        }

        logger.Warning("Unable to generate tokens for refreshToken {@Token}. Result: {@JwtResult}", req.RefreshToken, jwtResult);
        return TypedResults.BadRequest(new ProblemDetails { Detail = "Unable to generate tokens." });

    }
}
