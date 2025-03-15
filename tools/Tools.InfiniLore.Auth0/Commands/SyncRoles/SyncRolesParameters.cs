// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;

namespace Tools.InfiniLore.Auth0.Commands.SyncRoles;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct SyncRolesParameters : IParameters {
    [CliArgsParameter("api-identifier", "api")]
    [CliArgsDescription("Do Console Output")]
    public string ApiIdentifier { get; init; } = "https://localhost:7059/api";

}
