// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;

namespace DevTools.InfiniLore.Commands.DownloadRepo;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record DownloadRepoParameters : ICliParameters {
    [CliData("root", "r")]
    // [CliArgsDescription("The root directory of the project to update")]
    public string Root { get; init; } = "../../../../../";

    [CliData("output", "o")]
    // [CliArgsDescription("The root directory of the project to update")]
    public string OutputFolder { get; init; } = ".temp/";

    [CliData("link", "l")]
    // [CliArgsDescription("The root directory of the project to update")]
    public bool LinkToSolution { get; init; } = false;

    [CliData("solution", "s")]
    // [CliArgsDescription("The root solution file of the project to update")]
    public required string SolutionFile { get; init; }
}
