// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;

namespace Tools.InfiniLore.Commands.ModuleSetup;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record ModuleSetupParameters : ICliParameters {
    [CliData("root", "r")]
    // [CliArgsDescription("The root directory of the project to update")]
    public string Root { get; init; } = "../../../../../";

    public string SolutionFile => Path.Join(Root, "InfiniLore.sln");
}
