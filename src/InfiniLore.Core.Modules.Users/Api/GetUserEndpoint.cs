// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;

namespace InfiniLore.Core.Modules.Users.Api;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetUserEndpoint : EndpointWithoutRequest {
    public override void Configure() {
        Get("/api/users/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct) {
        await Send.OkAsync(null, ct);
    }
}
