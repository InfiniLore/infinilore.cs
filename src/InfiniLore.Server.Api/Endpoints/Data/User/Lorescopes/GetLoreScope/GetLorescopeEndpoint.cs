// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Database.Models.Data.User;
using InfiniLore.Server.Services.Auth0;
using InfiniLore.Server.Services.CQRS;
using InfiniLore.Server.Services.CQRS.Queries.Data.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Api.Endpoints.Data.User.Lorescopes.GetLoreScope;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok<LoreScopeResponse>,
    NotFound,
    ProblemDetails
>;

public class GetLorescopeEndpoint(IMediator mediator, ILogger<GetLorescopeEndpoint> logger) : Endpoint<GetLorescopeRequest, Response, LoreScopeMapper> {
    public override void Configure() {
        Get("/data/user/{UserId:guid}/lorescope/{LoreScopeId:guid}");
        Permissions(PermissionsStore.LorescopeRead);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Execute Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Response> ExecuteAsync(GetLorescopeRequest req, CancellationToken ct) {
        MediatorResponse<LoreScope> result = await mediator.Send(new GetLorescopeByIdQuery(req.LoreScopeId, req.UserId), ct);
        if (!result.TryGetAsSuccess(out LoreScope? loreScope)) {
            logger.Warning("Failed to get lorescope with id {id} because '{reason}'", req.LoreScopeId, result.AsError.Value);
            return TypedResults.NotFound();
        }

        logger.Information("Successfully retrieved lorescope with id {id}", req.LoreScopeId);

        LoreScopeResponse response = Map.FromEntity(loreScope);
        return TypedResults.Ok(response);
    }
}
