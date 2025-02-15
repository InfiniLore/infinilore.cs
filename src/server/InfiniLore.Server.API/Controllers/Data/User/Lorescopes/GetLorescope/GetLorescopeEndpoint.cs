// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Database.Models.Content.Data.User;
using InfiniLore.Server.Services.CQRS.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InfiniLore.Server.API.Controllers.Data.User.Lorescopes.GetLorescope;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using EndpointResult=Results<Ok<LorescopeResponse>, BadRequest<ProblemDetails>>;

public class GetLorescopeEndpoint(IMediator mediator) : Endpoint<GetLorescopeRequest, EndpointResult, GetLorescopeMapper> {

    public override void Configure() {
        Get("/data-user/{UserId:guid}/lore-scopes/{LorescopeId:guid}");
        // Permissions("data-user.lore-scopes.read");
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<EndpointResult> ExecuteAsync(GetLorescopeRequest req, CancellationToken ct) {
        SuccessOrFailure<LorescopeModel> data = await mediator.Send(
            new GetOneLorescopeQuery(req.LorescopeId),
            ct
        );

        if (data.TryGetAsFailureValue(out string? failureString)) {
            return TypedResults.BadRequest(new ProblemDetails {
                Detail = failureString
            });
        }

        return TypedResults.Ok(Map.FromEntity(data.AsSuccess.Value));
    }
}
