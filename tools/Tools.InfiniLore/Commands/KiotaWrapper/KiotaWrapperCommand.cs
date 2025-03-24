// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using JetBrains.Annotations;
using System.ComponentModel;
using System.Diagnostics;

namespace Tools.InfiniLore.Commands.KiotaWrapper;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[CliArgsCommand("kiota-wrapper")]
public partial class KiotaWrapperCommand : ICommand<KiotaWrapperParameters> {
    /// <summary>
    ///     Executes the Kiota process to generate the OpenAPI client based on the provided parameters.
    /// </summary>
    /// <param name="parameters">
    ///     An instance of <see cref="KiotaWrapperParameters" /> containing necessary variables like namespace, language, and
    ///     output location.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    /// </returns>
    public async Task ExecuteAsync(KiotaWrapperParameters parameters) {
        // Resolve paths relative to the Root
        string root = Path.GetFullPath(parameters.Root);
        string outputFolder = Path.Combine(root, parameters.OutputFolder);
        string csprojPath = Path.Combine(root, parameters.CsprojPath);
        string tempCsprojPath = Path.Combine(root, ".temp/temp.csproj");

        // Ensure the .csproj is backed up
        Console.WriteLine("Backing up .csproj file...");
        BackupCsproj(csprojPath, tempCsprojPath);

        // Generate the Kiota client
        Console.WriteLine("Generating the OpenAPI client...");
        await RunKiotaGenerateAsync(parameters, outputFolder);

        // Restore the .csproj file if needed
        Console.WriteLine("Restoring the .csproj file...");
        RestoreCsprojIfNeeded(csprojPath, tempCsprojPath);

        // Run NuGet restore
        Console.WriteLine("Restoring NuGet packages...");
        await RunDotNetRestoreAsync(csprojPath);

        Console.WriteLine("Kiota generation completed successfully.");
    }

    private static void BackupCsproj(string csprojPath, string tempCsprojPath) {
        // Ensure backup folder exists
        string? tempDirectory = Path.GetDirectoryName(tempCsprojPath);
        if (tempDirectory is null) throw new Exception("Failed to get temp directory.");

        if (!Directory.Exists(tempDirectory)) Directory.CreateDirectory(tempDirectory);

        // Backup the .csproj file if it exists
        if (!File.Exists(csprojPath)) return;

        File.Copy(csprojPath, tempCsprojPath, true);
    }

    private static void RestoreCsprojIfNeeded(string csprojPath, string tempCsprojPath) {
        if (File.Exists(csprojPath) || !File.Exists(tempCsprojPath)) {
            Console.WriteLine("No restoration needed: .csproj exists or backup not found.");
            return;
        }

        File.Copy(tempCsprojPath, csprojPath);
        Console.WriteLine("Successfully restored .csproj file.");
    }

    private static async Task RunKiotaGenerateAsync(KiotaWrapperParameters parameters, string resolvedOutputFolder) {
        try {
            string arguments =
                "generate "
                + $"--openapi {parameters.OpenApiFile} "
                + "--language CSharp "
                + $"--namespace-name {parameters.NamespaceName} "
                + $"--class-name {parameters.ClassName} "
                + $"--output {resolvedOutputFolder} "
                + "--backing-store false "
                + "--exclude-backward-compatible "
                + "--clean-output --clear-cache";

            await ExecuteCommandAsync("kiota", arguments, resolvedOutputFolder);
        }
        catch (Win32Exception ex) {
            Console.WriteLine("Failed to run Kiota, this is most likely due to a missing kiota as a global tool.");
            Console.WriteLine("To install Kiota, run the following command:");
            Console.WriteLine("dotnet tool install --global Microsoft.OpenApi.Kiota");
            throw;
        }
    }

    private static async Task RunDotNetRestoreAsync(string csprojPath)
        => await ExecuteCommandAsync("dotnet", $"restore \"{csprojPath}\"");

    private static async Task ExecuteCommandAsync(string fileName, string arguments, string? workingDirectory = null) {
        var processInfo = new ProcessStartInfo(fileName, arguments) {
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using Process? process = Process.Start(processInfo);
        Console.WriteLine(await process?.StandardOutput.ReadToEndAsync()!);
        await process.WaitForExitAsync();

        if (process.ExitCode == 0) return;

        string error = await process.StandardError.ReadToEndAsync();
        throw new Exception($"Command failed: {fileName} {arguments}\nError: {error}");
    }
}
