// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;

namespace Tools.InfiniLore.Auth0.Commands.SyncRoles;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record SyncRolesParameters : ICliParameters {
    [CliData("api-identifier", "api")]
    // [CliArgsDescription("Do Console Output")]
    public string ApiIdentifier { get; init; } = "https://localhost:7059/api";

}
