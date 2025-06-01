// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;

namespace DevTools.InfiniLore.Commands.KiotaWrapper;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record KiotaWrapperParameters : ICliParameters {
    [CliData("root", "r")]
    // [CliArgsDescription("The root directory of the project to update")]
    public string Root { get; init; } = "../../../../../";

    [CliData("namespace", "n")]
    // [CliArgsDescription("The namespace for the generated code")]
    public required string NamespaceName { get; init; }

    [CliData("output", "o")]
    // [CliArgsDescription("The output folder for the generated client")]
    public required string OutputFolder { get; init; }

    [CliData("classname", "c")]
    // [CliArgsDescription("The class name for the generated client")]
    public required string ClassName { get; init; }

    [CliData("openapi", "f")]
    // [CliArgsDescription("The OpenAPI file location (URL or local path)")]
    public required string OpenApiFile { get; init; }

    [CliData("csproj", "p")]
    // [CliArgsDescription("The project file path (.csproj) to back up and restore")]
    public required string CsprojPath { get; init; }
}
