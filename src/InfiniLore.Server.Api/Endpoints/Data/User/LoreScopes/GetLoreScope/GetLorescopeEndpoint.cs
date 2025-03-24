// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using FluentValidation;
using InfiniLore.Server.Api.Mappers.Data.User.LoreScopes;
using InfiniLore.Server.Api.Responses.Data.User.LoreScopes;
using InfiniLore.Server.Contracts.Services.Auth0;
using InfiniLore.Server.Database.Models.Data.User;
using InfiniLore.Server.Services.CQRS;
using InfiniLore.Server.Services.CQRS.Queries.Data.User;
using InfiniLore.ServerClient.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Api.Endpoints.Data.User.LoreScopes.GetLoreScope;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok<LoreScopeResponse>,
    NotFound,
    UnauthorizedHttpResult,
    ProblemDetails
>;

public class GetLorescopeEndpoint(
    IMediator mediator,
    ILogger<GetLorescopeEndpoint> logger,
    IJwtTokenHelper jwtTokenHelper
) : Endpoint<GetLorescopeRequest, Response, LoreScopeMapper> {
    public override void Configure() {
        Get("/data/user/{UserId:guid}/lorescope/{LoreScopeId:guid}");
        Permissions(PermissionsStore.LorescopeRead);
        Policies(ApiPolicies.JwtProtected);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Execute Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Response> ExecuteAsync(GetLorescopeRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();

        // Form Query
        var query = new GetLorescopeByIdQuery(
            req.LoreScopeId,
            req.UserId
        ) {
            AccessData = await RequestAccessData.FromJwtTokenAsync(jwtTokenHelper, ct)
        };
        
        // Execute Query
        MediatorResponse<LoreScope> result = await mediator.Send(query, ct);

        // Verify Response
        if (!result.TryGetAsSuccess(out LoreScope? loreScope)) {
            logger.Warning("Failed to get lorescope with id {id} because '{reason}'", req.LoreScopeId, result.AsError.Value);
            return TypedResults.NotFound();
        }

        logger.Information("Successfully retrieved lorescope with id {id}", req.LoreScopeId);
        
        // Return
        LoreScopeResponse response = Map.FromEntity(loreScope);
        return TypedResults.Ok(response);
    }
}
