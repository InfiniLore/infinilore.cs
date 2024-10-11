// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Services;
using InfiniLore.Server.Data.Models.Account;
using InfiniLoreLib.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ProblemDetails=FastEndpoints.ProblemDetails;

namespace InfiniLore.Server.API.Controllers.Account.JWT;
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
        Post("/account/jwt/tokens-create");
        AllowAnonymous();
    }

    public async override Task<Results<BadRequest<ProblemDetails>, Ok<JwtResponse>>> ExecuteAsync(JwtCreateTokensRequest req, CancellationToken ct) {
        if (await signInManager.UserManager.FindByNameAsync(req.Username) is not {} user) return TypedResults.BadRequest(new ProblemDetails {Detail = "Invalid username"});
        if (!await signInManager.CanSignInAsync(user)) return TypedResults.BadRequest(new ProblemDetails {Detail = "Unable to sign in."});
        if (await signInManager.CheckPasswordSignInAsync(user, req.Password, false) is not { Succeeded: true }) TypedResults.BadRequest(new ProblemDetails {Detail = "Invalid username or password."});

        JwtResult jwtResult = await jwtTokenService.GenerateTokensAsync(user, [], [], ct);
        if (jwtResult.IsFailure) {
            logger.Warning("{@result}", jwtResult);
            return TypedResults.BadRequest(new ProblemDetails {Detail = "Unable to generate tokens."});
        }
        
        return TypedResults.Ok(new JwtResponse(
            AccessToken : jwtResult.AccessToken!,
            AccessTokenExpiryUtc : (DateTime)jwtResult.AccessTokenExpiryUtc!,
            RefreshToken : (Guid)jwtResult.RefreshToken!,
            RefreshTokenExpiryUtc : (DateTime)jwtResult.RefreshTokenExpiryUtc!
        ));
    }
}
