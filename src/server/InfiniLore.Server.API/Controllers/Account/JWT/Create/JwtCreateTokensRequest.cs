// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.API.Controllers.Account.JWT.Create;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class JwtCreateTokensRequest {
    public required string Username { get; [UsedImplicitly] init; }
    public required string Password { get; [UsedImplicitly] init; }
    public string[] Roles { get; [UsedImplicitly] init; } = [];
    public string[] Permissions { get; [UsedImplicitly] init; } = [];
    public int? RefreshExpiresInDays { get; [UsedImplicitly] init; } = null;
}
