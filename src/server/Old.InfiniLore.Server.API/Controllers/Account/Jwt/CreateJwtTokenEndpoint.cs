// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using Old.InfiniLore.Contracts.Services;
using Old.InfiniLore.Database.Models.Content.Account;
using Old.InfiniLore.Server.Services.CQRS.Requests.Commands.Account;
using Old.InfiniLore.Server.Services.CQRS.Requests.Commands.Account.Jwt;
using Old.InfiniLore.Server.Types;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Old.InfiniLore.Server.API.Controllers.Account.Jwt;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

// REWORK OF https://github.com/InfiniLore/infinilore.cs/blob/75d6056ef32fe8aaf2ce5d88ad5f726c4e5c40db/src/server/InfiOld.InfiniLore.Server.APItrollers/Account/JWT/Create/JwtCreateTokensEndpoint.cs

public class JwtCreateTokenEndpoint(IMediator mediator, IMediatorOutputService mediatorOutput)
    : Endpoint<
        CreateJwtTokenRequest,
        Results<Ok<JwtTokenResponse>, BadRequest<ProblemDetails>>
    > {

    public override void Configure() {
        Post("/account/jwt/create-token/");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<JwtTokenResponse>, BadRequest<ProblemDetails>>> ExecuteAsync(CreateJwtTokenRequest req, CancellationToken ct) {
        SuccessOrFailure<InfiniLoreUser> resultLogin = await mediator.Send(new LoginCommand(req.Username, req.Password), ct);
        if (resultLogin.TryGetAsFailureValue(out string? loginFailure)) return mediatorOutput.ToBadRequest<JwtTokenResponse>(loginFailure);

        InfiniLoreUser user = resultLogin.AsSuccess.Value;
        SuccessOrFailure<JwtTokenData> resultToken = await mediator.Send(new CreateJwtTokenCommand(
            user,
            req.Roles,
            req.Permissions,
            req.RefreshExpiresInDays
        ), ct);

        return mediatorOutput.ToHttpResults(resultToken, mapper: data => JwtTokenResponse.FromModel(user, data));
    }
}
