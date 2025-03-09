// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;

namespace Tools.InfiniLore.Auth0.Commands.SyncPermissions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct SyncPermissionsParameters : IParameters {
    [CliArgsParameter("api-identifier", "api")]
    [CliArgsDescription("Do Console Output")]
    public string ApiIdentifier { get; init; } = "https://localhost:7059/api";

}
