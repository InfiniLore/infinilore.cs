// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Api.Mappers.Data.User.LoreScopes;
using InfiniLore.Server.Api.Responses.Data.User.LoreScopes;
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Database.Models.Data.User;
using InfiniLore.Server.Services.CQRS;
using InfiniLore.Server.Services.CQRS.Queries.Data.User;
using InfiniLore.ServerClient.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Api.Endpoints.Data.User.LoreScopes.GetLoreScopes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok<LoreScopesResponse>,
    NotFound,
    ProblemDetails
>;

public class GetLoreScopesEndpoint(IMediator mediator, ILogger<GetLoreScopesEndpoint> logger) : Endpoint<GetLoreScopesRequest, Response, LoreScopesMapper> {
    public override void Configure() {
        Get("/data/user/{UserId:guid}/lorescope");
        Permissions(PermissionsStoreConstants.LorescopeRead);
        Policies(ApiPolicies.JwtProtected);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Execute Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Response> ExecuteAsync(GetLoreScopesRequest req, CancellationToken ct) {
        MediatorResponse<PaginatedData<LoreScope>> result = await mediator.Send(new GetLoreScopesQuery(req.UserId, false, new PaginationInfo(1)), ct);
        if (!result.TryGetAsSuccess(out PaginatedData<LoreScope> paginatedResult)) {
            logger.Warning("Failed to get LoreScopes for user {userId} because '{reason}'", req.UserId, result.AsError.Value);
            return TypedResults.NotFound();
        }

        logger.Information("Successfully retrieved LoreScopes for userId {id}", req.UserId);

        LoreScopesResponse response = Map.FromEntity(paginatedResult);
        return TypedResults.Ok(response);
    }
}
