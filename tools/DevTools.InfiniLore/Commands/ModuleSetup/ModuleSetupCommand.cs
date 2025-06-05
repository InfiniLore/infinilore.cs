// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using DevTools.InfiniLore.Library;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace DevTools.InfiniLore.Commands.ModuleSetup;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[CliData("module-setup")]
public partial class ModuleSetupCommand(
    ILogger<ModuleSetupCommand> logger,
    CliHelper cliHelper,
    TemplateHelper templateHelper
) : ICliCommand<ModuleSetupParameters> {
    private static readonly Dictionary<string, string[]> SubProjects = new() {
        ["Server"] = [
            "CodeOfChaos.Extensions.DependencyInjection.Generators",
            "CodeOfChaos.Extensions.MicrosoftLogging",
        ],
        ["Server.Contracts"] = [],
        ["Shared"] = [
            "CodeOfChaos.Extensions.DependencyInjection",
            "CodeOfChaos.Extensions.DependencyInjection.Generators",
        ],
        ["Wasm"] = [
            "CodeOfChaos.Extensions.MicrosoftLogging",
            "CodeOfChaos.Extensions.DependencyInjection",
            "CodeOfChaos.Extensions.DependencyInjection.Generators",
            "Microsoft.Extensions.Logging.Abstractions",
            "Microsoft.AspNetCore.Components.WebAssembly",
        ]
    };

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask ExecuteAsync(ModuleSetupParameters parameters, CancellationToken ct = new()) {
        logger.Information("Starting module setup");

        string moduleName = GetNewModuleName();
        await CreateModuleProjects(moduleName, parameters, ct);
        
        await CreateEntryInterface(moduleName, "Server", parameters, ct);
        await CreateEntryInterface(moduleName, "Wasm", parameters, ct);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private string GetNewModuleName() {
        for (int i = 0; i < 5; i++) {
            logger.Information("Please give the name fore the new module (try {i}/5):", i);
            string? moduleName = Console.ReadLine();

            // ReSharper disable once InvertIf
            if (!moduleName.IsNullOrWhiteSpace()) return moduleName;
            logger.Error("No valid module name given");
        }
        
        logger.Error("No valid module name given after 5 tries");
        throw new Exception("No valid module name given after 5 tries");
    }

    private string GetModuleProjectName(string moduleName, string section) {
        string projectName = $"InfiniLore.Modules.{moduleName}.{section}";
        return projectName;
    }

    private string GetModuleOutputDir(string moduleName, string section) {
        string projectName = GetModuleProjectName(moduleName, section);
        string outputDir = Path.Combine("src", projectName);
        return outputDir;
    }

    private async ValueTask CreateModuleProjects(string moduleName, ModuleSetupParameters parameters, CancellationToken ct = default) {
        foreach (string section in SubProjects.Keys) {
            string projectName = GetModuleProjectName(moduleName, section);
            string outputDir = GetModuleOutputDir(moduleName, section);

            // Create the project using dotnet CLI
            await cliHelper.ExecuteCommandAsync("dotnet", $"new classlib -n {projectName} -o \"{outputDir}\"", parameters.Root, ct);
            string class1Path = Path.Combine(outputDir, "Class1.cs");
            if (File.Exists(class1Path)) File.Delete(class1Path);

            // Add it to the solution
            string csprojPath = Path.Combine("src", projectName, $"{projectName}.csproj");
            await cliHelper.ExecuteCommandAsync("dotnet", $"sln add \"{csprojPath}\" --solution-folder \"src/Server/Modules/{moduleName}\"", parameters.Root, ct);

            // Optional:  Add Nuget Packages
            foreach (string package in SubProjects[section]) {
                await cliHelper.ExecuteCommandAsync("dotnet", $"add \"{csprojPath}\" package {package}", parameters.Root, ct);
            }
            
            await Task.Delay(1000, ct);
        }
    }

    private async ValueTask CreateEntryInterface(string moduleName, string mode, ModuleSetupParameters parameters, CancellationToken ct = default) {
        string generatedCode = await templateHelper.LoadTemplateModuleSetupFileAsync(moduleName, mode, ct);
        string interfaceFileName = $"IModule{moduleName}Server.cs";
        string outputDir = GetModuleOutputDir(moduleName, mode);
        string interfaceFilePath = Path.Combine(parameters.Root, outputDir, interfaceFileName);
        await File.WriteAllTextAsync(interfaceFilePath, generatedCode, ct);
    }
}
