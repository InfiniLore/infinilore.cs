// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Services;
using InfiniLore.Server.Data.Models.Account;
using InfiniLoreLib.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace InfiniLore.Server.API.Controllers.Account.JWT.Revoke;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class JwtRevokeAllTokensEndpoint(IJwtTokenService jwtTokenService, ILogger logger, UserManager<InfiniLoreUser> userManager) 
    : EndpointWithoutRequest<
        Results<
            BadRequest<ProblemDetails>,
            Ok
        >
    > {
    
    public override void Configure() {
        Delete("/account/jwt/token/refresh/all");
        PermissionsAll("account.jwt.tokens_revoke");
    }
    
    public async override Task<Results<BadRequest<ProblemDetails>, Ok>> ExecuteAsync(CancellationToken ct) {
        if ( await userManager.GetUserAsync(User) is not {} user) {
            return TypedResults.BadRequest(new ProblemDetails { Detail = "User not found." });
        }

        BoolResult boolResult = await jwtTokenService.RevokeAllTokensFromUserAsync(user, ct);
        if (!boolResult.IsFailure) return TypedResults.Ok();

        logger.Warning("Unable to revoke tokens. Result: {@BoolResult}", boolResult.ErrorMessage);
        return TypedResults.BadRequest(new ProblemDetails { Detail = "Tokens could not be revoked." });
    }
}
