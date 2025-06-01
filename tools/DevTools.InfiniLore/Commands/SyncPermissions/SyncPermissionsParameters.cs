// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;

namespace DevTools.InfiniLore.Commands.SyncPermissions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record SyncPermissionsParameters : ICliParameters {
    [CliData("api-identifier", "api")]
    // [CliArgsDescription("Do Console Output")]
    public string ApiIdentifier { get; init; } = "https://localhost:7059/api";

}
