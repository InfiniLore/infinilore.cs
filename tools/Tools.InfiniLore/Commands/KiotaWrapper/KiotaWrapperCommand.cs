// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using Tools.InfiniLore.Library;

namespace Tools.InfiniLore.Commands.KiotaWrapper;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[CliData("kiota-wrapper")]
public partial class KiotaWrapperCommand(
    ILogger<KiotaWrapperCommand> logger,
    CliHelper cliHelper
) : ICliCommand<KiotaWrapperParameters> {

    public async ValueTask ExecuteAsync(KiotaWrapperParameters parameters, CancellationToken ct = default) {
        // Resolve paths relative to the Root
        string root = Path.GetFullPath(parameters.Root);
        string outputFolder = Path.Combine(root, parameters.OutputFolder);
        string csprojPath = Path.Combine(root, parameters.CsprojPath);
        string tempCsprojPath = Path.Combine(root, ".temp/temp.csproj");

        // Ensure the .csproj is backed up
        logger.Information("Backing up .csproj file...");
        BackupCsproj(csprojPath, tempCsprojPath);

        // Generate the Kiota client
        logger.Information("Generating the OpenAPI client...");
        await RunKiotaGenerateAsync(parameters, outputFolder);

        // Restore the .csproj file if needed
        logger.Information("Restoring the .csproj file...");
        RestoreCsprojIfNeeded(csprojPath, tempCsprojPath);

        // Run NuGet restore
        logger.Information("Restoring NuGet packages...");
        await RunDotNetRestoreAsync(csprojPath);
        logger.Information("Initial Kiota client generated");
        
        logger.Information("Starting Post Processing");
        await RunPostProcessingAsync(parameters, csprojPath);
    }
    
    private async ValueTask RunPostProcessingAsync(KiotaWrapperParameters parameters, string csprojPath) {
        logger.Information("Renaming classes to Kiota");
        await ReplaceLongKiotaClassNamesAsync(csprojPath, [
            "InfiniLoreModulesCoreServerApiEndpoints",
            "InfiniLoreModulesLoreScopesServerApiEndpoints"
        ]);
        
        logger.Information("Fixing specific lines in generated files");
        Dictionary<string, (int Line, string Replacement)[]> data = new() {
            ["src/InfiniLore.Kiota/Models/KiotaLoreScopeResponse.cs"] = [
                (16, "        public new IDictionary<string, object> AdditionalData { get; set; }")
            ]
        };

        IEnumerable<Task> tasks = data.Select(pair => FixSpecificFileIssues(Path.Join(parameters.Root, pair.Key), pair.Value));
        await Task.WhenAll(tasks);
        
        // End
        logger.Information("Post Processing completed");
    }

    private void BackupCsproj(string csprojPath, string tempCsprojPath) {
        // Ensure backup folder exists
        string? tempDirectory = Path.GetDirectoryName(tempCsprojPath);
        if (tempDirectory is null) {
            logger.Error("Failed to get temp directory at {temp}", tempCsprojPath);
            throw new Exception("Failed to get temp directory.");
        }

        if (!Directory.Exists(tempDirectory)) Directory.CreateDirectory(tempDirectory);

        // Back up the .csproj file if it exists
        if (!File.Exists(csprojPath)) return;

        File.Copy(csprojPath, tempCsprojPath, true);
    }

    private void RestoreCsprojIfNeeded(string csprojPath, string tempCsprojPath) {
        if (File.Exists(csprojPath) || !File.Exists(tempCsprojPath)) {
            logger.Information("No restoration needed: .csproj exists or backup not found.");
            return;
        }

        File.Copy(tempCsprojPath, csprojPath);
        logger.Information("Restored .csproj file from backup.");
    }

    private async Task RunKiotaGenerateAsync(KiotaWrapperParameters parameters, string resolvedOutputFolder) {
        try {
            string arguments =
                "generate "
                + $"--openapi {parameters.OpenApiFile} "
                + "--language CSharp "
                + $"--namespace-name {parameters.NamespaceName} "
                + $"--class-name {parameters.ClassName} "
                + $"--output {resolvedOutputFolder} "
                + "--backing-store false "
                + "--exclude-backward-compatible true "
                + "--additional-data true "
                + "--clean-output "
                + "--clear-cache";

            await cliHelper.ExecuteCommandAsync("kiota", arguments, resolvedOutputFolder);
        }
        catch (Win32Exception ) {
            logger.Error("Failed to run Kiota, this is most likely due to a missing kiota as a global tool. To install Kiota, run the following command: {cmd}", "dotnet tool install --global Microsoft.OpenApi.Kiota");
            throw;
        }
    }

    private async Task RunDotNetRestoreAsync(string csprojPath)
        => await cliHelper.ExecuteCommandAsync("dotnet", $"restore \"{csprojPath}\"");

    private async Task ReplaceLongKiotaClassNamesAsync(string csprojPath, string[] namesToReplace) {
        // collect all .cs files
        string projectDirectory = Path.GetDirectoryName(csprojPath)!;
        List<string> files = Directory.GetFiles(projectDirectory, "*.cs", SearchOption.AllDirectories)
            .ToList();

        await Parallel.ForEachAsync(files, async (fileName, ct) => {
            bool isFileChanged = false;
            string fileContent = await File.ReadAllTextAsync(fileName, ct);
            foreach (string name in namesToReplace) {
                if (!fileContent.Contains(name)) continue;

                fileContent = fileContent.Replace(name, "Kiota");
                isFileChanged = true;
            }

            if (!isFileChanged) return;

            await File.WriteAllTextAsync(fileName, fileContent, ct);
            logger.Information("Replaced long Kiota class names in {fileName}", fileName);
            
            foreach (string name in namesToReplace) {
                if (!fileName.Contains(name)) continue;
                
                string newFileName = fileName.Replace(name, "Kiota");
                File.Move(fileName, newFileName, true);
                logger.Information("Renamed {fileName} to {newFileName}", fileName, newFileName);
            }
        });
    }

    private async Task FixSpecificFileIssues(string fileName, IReadOnlyCollection<(int Line, string Replacement)> replacements) {
        // Check if file exists first
        if (!File.Exists(fileName)) {
            logger.Warning("File not found: {fileName}, skipping modifications", fileName);
            return;
        }
        
        // Read all lines from the file
        string[] lines = await File.ReadAllLinesAsync(fileName);
        bool fileChanged = false;

        // Process each replacement
        foreach ((int Line, string Replacement) replacement in replacements) {
            // Check if the line number is valid (remember that Line is 1-based, array is 0-based)
            if (replacement.Line <= 0 || replacement.Line > lines.Length) {
                logger.Warning("Invalid line number {line} for file {fileName}", replacement.Line, fileName);
                continue;
            }

            // Replace the line (adjusting for 0-based array index)
            int index = replacement.Line - 1;
            if (lines[index] != replacement.Replacement) {
                lines[index] = replacement.Replacement;
                fileChanged = true;
            }
        }

        // Only write the file if changes were made
        if (fileChanged) {
            await File.WriteAllLinesAsync(fileName, lines);
            logger.Information("Updated specific lines in {fileName}", fileName);
        }
    }
}
