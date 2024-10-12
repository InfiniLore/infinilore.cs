// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Services;
using InfiniLore.Server.Data.Models.Account;
using InfiniLoreLib.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace InfiniLore.Server.API.Controllers.Account.JWT.Revoke;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class JwtRevokeTokensEndpoint(IJwtTokenService jwtTokenService, ILogger logger, UserManager<InfiniLoreUser> userManager) 
    : Endpoint<
        JwtRevokeTokensRequest,
        Results<
            BadRequest<ProblemDetails>,
            Ok
        >
    > {
    public override void Configure() {
        Post("/account/jwt/tokens-revoke");
        PermissionsAll("account.jwt.tokens_revoke");
    }
    public async override Task<Results<BadRequest<ProblemDetails>, Ok>> ExecuteAsync(JwtRevokeTokensRequest req, CancellationToken ct) {
        if ( await userManager.FindByIdAsync(User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value) is not {} user) {
            return TypedResults.BadRequest(new ProblemDetails { Detail = "User not found." });
        }

        List<ProblemDetails.Error> errors = [];
        foreach (Guid refreshToken in req.RefreshTokens) {
            BoolResult boolResult = await jwtTokenService.RevokeTokensAsync(user, refreshToken, ct);
            if (!boolResult.IsFailure) continue;

            logger.Warning("Unable to revoke tokens for refreshToken {@Token}. Result: {@BoolResult}", refreshToken, boolResult.ErrorMessage);
            errors.Add(new ProblemDetails.Error {
                Name = "Unable to revoke tokens.",
                Reason = boolResult.ErrorMessage ?? string.Empty}
            );
        }
        
        if (errors.IsNullOrEmpty()) return TypedResults.Ok();
        return TypedResults.BadRequest(new ProblemDetails { Detail = "Unable to revoke tokens.", Errors = errors });
    }
}
