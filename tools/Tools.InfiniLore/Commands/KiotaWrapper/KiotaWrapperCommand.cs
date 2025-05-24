// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Diagnostics;

namespace Tools.InfiniLore.Commands.KiotaWrapper;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[CliData("kiota-wrapper")]
public partial class KiotaWrapperCommand(ILogger<KiotaWrapperCommand> logger) : ICliCommand<KiotaWrapperParameters> {

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

            await ExecuteCommandAsync("kiota", arguments, resolvedOutputFolder);
        }
        catch (Win32Exception ) {
            logger.Error("Failed to run Kiota, this is most likely due to a missing kiota as a global tool. To install Kiota, run the following command: {cmd}", "dotnet tool install --global Microsoft.OpenApi.Kiota");
            throw;
        }
    }

    private async Task RunDotNetRestoreAsync(string csprojPath)
        => await ExecuteCommandAsync("dotnet", $"restore \"{csprojPath}\"");

    private async Task ExecuteCommandAsync(string fileName, string arguments, string? workingDirectory = null) {
        var processInfo = new ProcessStartInfo(fileName, arguments) {
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        logger.LogInformation("Running command: {cmd}", $"{fileName} {arguments}");
        using Process? process = Process.Start(processInfo);
        
        logger.LogInformation("Command output: {output}", await process?.StandardOutput.ReadToEndAsync()!);
        await process.WaitForExitAsync();

        if (process.ExitCode != 0) {
            string error = await process.StandardError.ReadToEndAsync();
            logger.Error("Command failed: {error}", error);
            throw new Exception($"Command failed: {fileName} {arguments}\nError: {error}");
        }

    }
}
