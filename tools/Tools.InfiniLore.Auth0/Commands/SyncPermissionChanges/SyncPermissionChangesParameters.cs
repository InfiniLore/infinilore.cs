// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;

namespace Tools.InfiniLore.Auth0.Commands.SyncPermissionChanges;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct SyncPermissionChangesParameters : IParameters {
    [CliArgsParameter("output", "o")]
    [CliArgsDescription("Do Console Output")]
    public bool OutputFolder { get; init; } = true;

}
