// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace InfiniLore.Server.API.Controllers.Account.JWT;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class JwtCreateTokensRequest {
    public required string Username { get; [UsedImplicitly] init; }
    public required string Password { get; [UsedImplicitly] init; }
}
