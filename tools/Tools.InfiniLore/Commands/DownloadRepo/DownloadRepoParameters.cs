// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;

namespace Tools.InfiniLore.Commands.DownloadRepo;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct DownloadRepoParameters : IParameters {
    [CliArgsParameter("root", "r")]
    [CliArgsDescription("The root directory of the project to update")]
    public string Root { get; init; } = "../../../../../";

    [CliArgsParameter("output", "o")]
    [CliArgsDescription("The root directory of the project to update")]
    public string OutputFolder { get; init; } = ".temp/";

    [CliArgsParameter("link", "l")]
    [CliArgsDescription("The root directory of the project to update")]
    public bool LinkToSolution { get; init; } = false;

    [CliArgsParameter("solution", "s")]
    [CliArgsDescription("The root solution file of the project to update")]
    public required string SolutionFile { get; init; }
}
