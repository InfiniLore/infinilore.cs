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
    TemplateHelper templateHelper,
    CsprojHelper csprojHelper
) : ICliCommand<ModuleSetupParameters> {
    private static readonly Dictionary<string, string[]> NuGetReferences = new() {
        ["Server"] = [
            "CodeOfChaos.Extensions.DependencyInjection.Generators",
            "CodeOfChaos.Extensions.MicrosoftLogging",
        ],
        ["Server.Contracts"] = [],
        ["Shared"] = [
            "CodeOfChaos.Extensions.DependencyInjection",
            "CodeOfChaos.Extensions.DependencyInjection.Generators",
        ],
        ["Shared.Contracts"] = [],
        ["Wasm"] = [
            "CodeOfChaos.Extensions.MicrosoftLogging",
            "CodeOfChaos.Extensions.DependencyInjection",
            "CodeOfChaos.Extensions.DependencyInjection.Generators",
            "Microsoft.Extensions.Logging.Abstractions",
            "Microsoft.AspNetCore.Components.WebAssembly",
        ],
        ["Wasm.Contracts"] = [],
    };
    
    private static readonly Dictionary<string, string[]> ProjectReferences = new() {
        ["Server"] = [
            "InfiniLore.Modules.Core.Server",
            "InfiniLore.Modules.Core.Server.Contracts"
        ],
        ["Server.Contracts"] = [
            "InfiniLore.Modules.Core.Server.Contracts"
        ],
        ["Shared"] = [
            "InfiniLore.Modules.Core.Shared"
        ],
        ["Shared.Contracts"] = [
            "InfiniLore.Modules.Core.Shared.Contracts"
        ],
        ["Wasm"] = [
            "InfiniLore.Modules.Core.Wasm",
            "InfiniLore.Modules.Core.Wasm.Contracts"
        ],
        ["Wasm.Contracts"] = [
            "InfiniLore.Modules.Core.Wasm.Contracts"
        ]
    };

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask ExecuteAsync(ModuleSetupParameters parameters, CancellationToken ct = new()) {
        logger.Information("Starting module setup");

        string moduleName = GetNewModuleName();
        await CreateModuleProjects(moduleName, parameters, ct);
        await AddModuleReferences(moduleName, parameters, ct);
        
        await CreateModuleEntry(moduleName, "Server", parameters, ct);
        await CreateModuleEntry(moduleName, "Wasm", parameters, ct);
        
        await CreateModuleSetup(moduleName, "Server", parameters, ct);
        await CreateModuleSetup(moduleName, "Wasm", parameters, ct);
        
        logger.Information("Module setup completed");
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
        foreach (string section in NuGetReferences.Keys) {
            string projectName = GetModuleProjectName(moduleName, section);
            string outputDir = GetModuleOutputDir(moduleName, section);
            string csprojPath = Path.Combine("src", projectName, $"{projectName}.csproj");

            // Create the project using dotnet CLI
            await cliHelper.ExecuteCommandAsync("dotnet", $"new classlib -n {projectName} -o \"{outputDir}\"", parameters.Root, ct);
        
            // Delete Class1.cs
            string class1Path = Path.Combine(parameters.Root, outputDir, "Class1.cs");
            try {
                if (File.Exists(class1Path)) {
                    logger.LogInformation("Attempting to delete {FilePath}", class1Path);
                    File.Delete(class1Path);
                    logger.LogInformation("Successfully deleted Class1.cs");
                } else {
                    logger.LogWarning("Class1.cs not found at expected path: {FilePath}", class1Path);
                }
            } catch (Exception ex) {
                logger.LogError(ex, "Failed to delete Class1.cs at {FilePath}", class1Path);
            }

            // Set RootNamespace for contract projects
            if (section.EndsWith(".Contracts")) {
                string fullCsprojPath = Path.Combine(parameters.Root, csprojPath);
                string baseNamespace = projectName[..^9]; // Remove ".Contracts"
                await csprojHelper.SetPropertyAsync(fullCsprojPath, "RootNamespace", baseNamespace, ct);
            }

            // Add it to the solution
            await cliHelper.ExecuteCommandAsync("dotnet", $"sln add \"{csprojPath}\" --solution-folder \"src/Server/Modules/{moduleName}\"", parameters.Root, ct);

            // Add NuGet packages
            foreach (string package in NuGetReferences[section]) {
                await cliHelper.ExecuteCommandAsync("dotnet", $"add \"{csprojPath}\" package {package}", parameters.Root, ct);
            }

            // Add project references
            if (ProjectReferences.TryGetValue(section, out var references)) {
                foreach (string reference in references) {
                    string referencePath = Path.Combine("src", reference, $"{reference}.csproj");
                    await cliHelper.ExecuteCommandAsync("dotnet", $"add \"{csprojPath}\" reference \"{referencePath}\"", parameters.Root, ct);
                }
            }
        
            await Task.Delay(1000, ct);
        }
    }

    private async ValueTask AddModuleReferences(string moduleName, ModuleSetupParameters parameters, CancellationToken ct) {
        var references = new Dictionary<string, string[]> {
            ["Server"] = [
                $"InfiniLore.Modules.{moduleName}.Server.Contracts",
                $"InfiniLore.Modules.{moduleName}.Shared",
                $"InfiniLore.Modules.{moduleName}.Shared.Contracts"
            ],
            ["Wasm"] = [
                $"InfiniLore.Modules.{moduleName}.Wasm.Contracts",
                $"InfiniLore.Modules.{moduleName}.Shared",
                $"InfiniLore.Modules.{moduleName}.Shared.Contracts"
            ],
            ["Shared"] = [
                $"InfiniLore.Modules.{moduleName}.Shared.Contracts"
            ]
        };

        foreach ((string section, string[] contractRefs) in references) {
            string projectName = GetModuleProjectName(moduleName, section);
            string csprojPath = Path.Combine("src", projectName, $"{projectName}.csproj");

            foreach (string contractRef in contractRefs) {
                string contractPath = Path.Combine("src", contractRef, $"{contractRef}.csproj");
                logger.LogInformation("Adding contract reference {ContractRef} to {Project}", contractRef, projectName);
                await cliHelper.ExecuteCommandAsync("dotnet", $"add \"{csprojPath}\" reference \"{contractPath}\"", parameters.Root, ct);
            }
        }
    }

    private async ValueTask CreateModuleEntry(string moduleName, string mode, ModuleSetupParameters parameters, CancellationToken ct = default) {
        string generatedCode = await templateHelper.LoadTemplateModuleEntryFileAsync(moduleName, mode, ct);
        string interfaceFileName = $"IModule{moduleName}Server.cs";
        string outputDir = GetModuleOutputDir(moduleName, mode);
        string interfaceFilePath = Path.Combine(parameters.Root, outputDir, interfaceFileName);
        await File.WriteAllTextAsync(interfaceFilePath, generatedCode, ct);
    }
    
    private async ValueTask CreateModuleSetup(string moduleName, string mode, ModuleSetupParameters parameters, CancellationToken ct = default) {
        string generatedCode = await templateHelper.LoadTemplateModuleSetupFileAsync(moduleName, mode, ct);
        string interfaceFileName = $"{moduleName}Setup.cs";
        string outputDir = GetModuleOutputDir(moduleName, mode);
        string interfaceFilePath = Path.Combine(parameters.Root, outputDir, interfaceFileName);
        await File.WriteAllTextAsync(interfaceFilePath, generatedCode, ct);
    }
}
