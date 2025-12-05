// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Core.Outcomes;
using InfiniLore.Core.Pagination;
using InfiniLore.Modules.Users.Database;
using InfiniLore.Modules.Users.Messaging.Queries;

namespace InfiniLore.Modules.Users.Api;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetUsersEndpoint : Endpoint<GetUsersRequest, UsersResponse, UsersMapper> {
    public override void Configure() {
        Get("/api/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetUsersRequest req, CancellationToken ct) {

        var query = new GetUsersQuery {
            Pagination = new PaginationData(req.PageNumber)
        };
        PaginatedOutcome<UserModel> outcome = await query.ExecuteAsync(ct: ct);

        await outcome.SwitchAsync(
            async model => await Send.OkAsync(await Map.FromEntityAsync(model, ct), ct),
            async _ => await Send.ErrorsAsync(cancellation: ct)
        );

    }
}
