// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Core.Outcomes;
using InfiniLore.Modules.Users.Database;
using InfiniLore.Modules.Users.Messaging.Queries;

namespace InfiniLore.Modules.Users.Api;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetUsersEndpoint : Endpoint<GetUserRequest, UserResponse, UserMapper> {
    public override void Configure() {
        Get("/api/users/{UserId:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetUserRequest req, CancellationToken ct) {

        Outcome<UserModel> outcome = await GetUserQuery.FromUserId(req.UserId).ExecuteAsync(ct: ct);

        await outcome.SwitchAsync(
            async model => await Send.OkAsync(await Map.FromEntityAsync(model, ct), ct),
            async _ => await Send.ErrorsAsync(cancellation: ct)
        );

    }
}
