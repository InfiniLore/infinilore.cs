// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;

namespace DevTools.InfiniLore.Commands.SetServerSecrets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record SetServerSecretsParameters : ICliParameters {
    [CliData("root", "r")] public string Root { get; init; } = "../../../../../";

    [CliData("client-id-web-app", "ciwp")] public required string ClientIdWebApp { get; init; }
    [CliData("client-secret-web-app", "cswa")] public required string ClientSecretWebApp { get; init; }
    [CliData("domain", "d")] public required string Domain { get; init; }
    [CliData("api-identifier", "ai")] public string ApiIdentifier { get; init; } = "https://localhost:7059/api";
    [CliData("client-id-management", "cim")] public required string ClientIdManagement { get; init; }
    [CliData("client-secret-management", "csm")] public required string ClientSecretManagement  { get; init; }
    [CliData("audience", "a")] public string Audience { get; init; } = "https://localhost:7059/api";
    
    public string InfiniLoreServerFolder => Path.Join(Root, "src/InfiniLore.Server/");
    public string DevToolsInfiniLoreFolder => Path.Join(Root, "tools/DevTools.InfiniLore/");
}
