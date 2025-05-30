// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tools.InfiniLore.Library;

namespace Tools.InfiniLore.Commands.ModuleSetup;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[CliData("module-setup")]
public partial class ModuleSetupCommand(
    ILogger<ModuleSetupCommand> logger,
    CliHelper cliHelper
) : ICliCommand<ModuleSetupParameters>{
    public async ValueTask ExecuteAsync(ModuleSetupParameters parameters, CancellationToken ct = new()) {
        logger.Information("Starting module setup");
        
        string moduleName = GetNewModuleName();

        (string command, string arguments)[] commands = [
            ("dotnet", "sln add src/InfiniLore.Modules.{moduleName}.Server.csproj --solution-folder {moduleName}"),
            ("dotnet", "sln add src/InfiniLore.Modules.{moduleName}.Server.Contracts.csproj --solution-folder {moduleName}"),
            ("dotnet", "sln add src/InfiniLore.Modules.{moduleName}.Shared.csproj --solution-folder {moduleName}"),
            ("dotnet", "sln add src/InfiniLore.Modules.{moduleName}.Wasm.csproj --solution-folder {moduleName}"),
        ];
        
        foreach ((string command, string arguments) in commands) {
            await cliHelper.ExecuteCommandAsync(command, arguments.Replace("{moduleName}", moduleName), parameters.Root);
        }
        
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private string GetNewModuleName() {
        logger.Information("Please give the name fore the new module:");
        string? moduleName = Console.ReadLine();
        
        // ReSharper disable once InvertIf
        if (moduleName is null) {
            logger.Error("No module name given");
            throw new Exception("No module name given");
        }
        return moduleName;
    }
}
