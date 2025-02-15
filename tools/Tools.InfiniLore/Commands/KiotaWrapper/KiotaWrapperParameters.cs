// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;

namespace Tools.InfiniLore.Commands.KiotaWrapper;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct KiotaWrapperParameters : IParameters {
    [CliArgsParameter("root", "r")]
    [CliArgsDescription("The root directory of the project to update")]
    public string Root { get; init; } = "../../../../../";

    [CliArgsParameter("namespace", "n")]
    [CliArgsDescription("The namespace for the generated code")]
    public required string NamespaceName { get; init; }

    [CliArgsParameter("output", "o")]
    [CliArgsDescription("The output folder for the generated client")]
    public required string OutputFolder { get; init; }

    [CliArgsParameter("classname", "c")]
    [CliArgsDescription("The class name for the generated client")]
    public required string ClassName { get; init; }

    [CliArgsParameter("openapi", "f")]
    [CliArgsDescription("The OpenAPI file location (URL or local path)")]
    public required string OpenApiFile { get; init; }

    [CliArgsParameter("csproj", "p")]
    [CliArgsDescription("The project file path (.csproj) to back up and restore")]
    public required string CsprojPath { get; init; }
}
