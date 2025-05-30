// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Tools.InfiniLore.Library;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<CliHelper>]
public class CliHelper(ILogger<CliHelper> logger) {
    public async Task ExecuteCommandAsync(string fileName, string arguments, string? workingDirectory = null) {
        var processInfo = new ProcessStartInfo(fileName, arguments) {
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        logger.LogInformation("Running command: {cmd}", $"{fileName} {arguments}");
        using Process? process = Process.Start(processInfo);
        if (process == null) {
            logger.Error("Failed to start process to run {cmd}", $"{fileName} {arguments}");
            throw new Exception($"Failed to start process to run {fileName} {arguments}");
        }

        string output = await process.StandardOutput.ReadToEndAsync();
        logger.LogInformation("Command output: {output}", output);
        
        await process.WaitForExitAsync();

        if (process.ExitCode != 0) {
            string error = await process.StandardError.ReadToEndAsync();
            logger.Error("Command failed: {error}", error);
            throw new Exception($"Command failed: {fileName} {arguments}\nError: {error}");
        }
    }
}
