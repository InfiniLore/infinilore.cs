// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.API.Controllers.Account.Identity;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class IdentityCreateUserRequest {
    public required string Username { get; [UsedImplicitly] init; }
    public required string Password { get; [UsedImplicitly] init; }
    public required string Email { get; [UsedImplicitly] init; }
}
