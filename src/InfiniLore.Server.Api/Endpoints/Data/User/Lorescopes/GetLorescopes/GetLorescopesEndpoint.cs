// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Database.Models.Data.User;
using InfiniLore.Server.Services.Auth0;
using InfiniLore.Server.Services.CQRS;
using InfiniLore.Server.Services.CQRS.Queries.Data.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Api.Endpoints.Data.User.Lorescopes.GetLorescopes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok<LoreScopesResponse>,
    NotFound,
    ProblemDetails
>;

public class GetLorescopesEndpoint(IMediator mediator, ILogger<GetLorescopesEndpoint> logger) : Endpoint<GetLorescopesRequest, Response, LoreScopesMapper> {
    public override void Configure() {
        Get("/data/user/{UserId:guid}/lorescope");
        Permissions(PermissionsStore.LorescopeRead);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Execute Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Response> ExecuteAsync(GetLorescopesRequest req, CancellationToken ct) {
        MediatorResponse<PaginatedResult<LoreScope>> result = await mediator.Send(new GetLorescopesQuery(req.UserId, false, req.PaginationInfo), ct);
        if (!result.TryGetAsSuccess(out PaginatedResult<LoreScope> paginatedResult)) {
            logger.Warning("Failed to get lorescopes for user {userId} because '{reason}'", req.UserId, result.AsError.Value);
            return TypedResults.NotFound();
        }

        logger.Information("Successfully retrieved lorescopes for userId {id}", req.UserId);

        LoreScopesResponse response = Map.FromEntity(paginatedResult);
        return TypedResults.Ok(response);
    }
}
