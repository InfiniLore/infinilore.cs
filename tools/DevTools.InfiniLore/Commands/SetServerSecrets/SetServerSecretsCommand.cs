// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using DevTools.InfiniLore.Library;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace DevTools.InfiniLore.Commands.SetServerSecrets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[CliData(CommandName)]
public partial class SetServerSecretsCommand(
    ILogger<SetServerSecretsCommand> logger,
    CliHelper cliHelper
) : ICliCommand<SetServerSecretsParameters>{
    public const string CommandName = "set-server-secrets";
    
    public async ValueTask ExecuteAsync(SetServerSecretsParameters parameters, CancellationToken ct = new()) {
        
        logger.LogInformation("Setting server secrets");

        foreach (string serverLocation in (string[])[parameters.InfiniLoreServerFolder, parameters.DevToolsInfiniLoreFolder]) {
            await cliHelper.ExecuteCommandAsync("dotnet", "user-secrets init", serverLocation, ct);
            await cliHelper.ExecuteCommandAsync("dotnet",$"user-secrets set \"Auth0:ClientId-WebApp\" \"{parameters.ClientIdWebApp}\"", serverLocation, ct);
            await cliHelper.ExecuteCommandAsync("dotnet",$"user-secrets set \"Auth0:ClientSecret-WebApp\" \"{parameters.ClientSecretWebApp}\"", serverLocation, ct);
            await cliHelper.ExecuteCommandAsync("dotnet",$"user-secrets set \"Auth0:Domain\" \"{parameters.Domain}\"", serverLocation, ct);
            await cliHelper.ExecuteCommandAsync("dotnet",$"user-secrets set \"Auth0:ApiIdentifier\" \"{parameters.ApiIdentifier}\"", serverLocation, ct);
            await cliHelper.ExecuteCommandAsync("dotnet",$"user-secrets set \"Auth0:ClientId-Management\" \"{parameters.ClientIdManagement}\"", serverLocation, ct);
            await cliHelper.ExecuteCommandAsync("dotnet",$"user-secrets set \"Auth0:ClientSecret-Management\" \"{parameters.ClientSecretManagement}\"", serverLocation, ct);
            await cliHelper.ExecuteCommandAsync("dotnet",$"user-secrets set \"Auth0:Audience\" \"{parameters.Audience}\"", serverLocation, ct);
        }
        
        logger.LogInformation("Server secrets set");
    }
}