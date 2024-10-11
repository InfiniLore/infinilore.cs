// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Services;
using InfiniLore.Server.Data.Models.Account;
using InfiniLoreLib.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InfiniLore.Server.API.Controllers.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class JwtLoginEndpoint(SignInManager<InfiniLoreUser> signInManager, IJwtTokenService jwtTokenService)
    : Endpoint<
        JwtLoginRequest,
        Results<
            UnauthorizedHttpResult,
            Ok<JwtResponse>
    >
> {
    public override void Configure() {
        Post("/api/login");
        AllowAnonymous();
    }

    public async override Task<Results<UnauthorizedHttpResult, Ok<JwtResponse>>> ExecuteAsync(JwtLoginRequest req, CancellationToken ct) {
        if (await signInManager.UserManager.FindByNameAsync(req.Username) is not {} user) return TypedResults.Unauthorized();
        if (await signInManager.CanSignInAsync(user)) return TypedResults.Unauthorized();
        if (await signInManager.CheckPasswordSignInAsync(user, req.Password, false) is not { Succeeded: true }) TypedResults.Unauthorized();

        JwtResult jwtResult = await jwtTokenService.GenerateTokensAsync(user, [], [], ct);
        if (jwtResult.IsFailure) return TypedResults.Unauthorized();
        
        return TypedResults.Ok(new JwtResponse(
            AccessToken : jwtResult.AccessToken!,
            AccessTokenExpiryUtc : (DateTime)jwtResult.AccessTokenExpiryUtc!,
            RefreshToken : (Guid)jwtResult.RefreshToken!,
            RefreshTokenExpiryUtc : (DateTime)jwtResult.RefreshTokenExpiryUtc!
        ));
    }
}
