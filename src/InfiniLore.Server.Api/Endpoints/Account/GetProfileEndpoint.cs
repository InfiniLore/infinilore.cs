// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;

namespace InfiniLore.Server.Api.Endpoints.Account;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetProfileEndpoint : EndpointWithoutRequest{
    public override void Configure()
    {
        Get("/account/profile");
        Permissions("Profile_Read", "Profile_Update"); //permission claims can be enforced
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        //var claims = User.Claims.ToArray();
        //var accessToken = await HttpContext.GetTokenAsync("access_token");
        await SendStringAsync("ok! you have permission to see this...", cancellation: ct);
    }
}
