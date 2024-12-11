// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Database.Models.Content.Data.User;
using InfiniLore.Server.Contracts.Services;
using InfiniLore.Server.Services;
using InfiniLore.Server.Services.CQRS.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InfiniLore.Server.API.Controllers.Data.User.Lorescopes.CreateLorescope;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CreateLorescopeEndpoint(IMediator mediator, IMediatorOutputService mediatorOutput)
    : Endpoint<
        CreateLorescopeRequest,
        Results<Ok<LorescopeResponse>, BadRequest<ProblemDetails>>,
        CreateLorescopeMapper
    > {

    public override void Configure() {
        Post("/data-user/{UserId:guid}/lore-scopes/");
        Permissions(ApiPermissions.LorescopeWrite, ApiPermissions.LorescopeManage); // Remember, Permissions works as (a or b), PermissionsALl works as (a and b)
    }

    public async override Task<Results<Ok<LorescopeResponse>, BadRequest<ProblemDetails>>> ExecuteAsync(CreateLorescopeRequest req, CancellationToken ct) {
        SuccessOrFailure<LorescopeModel> result = await mediator.Send(
            new CreateLorescopeCommand(Map.ToEntity(req)),
            ct
        );

        return mediatorOutput.ToHttpResults(result, Map.FromEntity);
    }
}
