// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace InfiniLore.Server.API.Controllers.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class JwtLoginRequest {
    public required string Username { get; [UsedImplicitly] init; }
    public required string Password { get; [UsedImplicitly] init; }
}
