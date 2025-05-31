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
    public async Task ExecuteCommandAsync(string fileName, string arguments, string? workingDirectory = null, CancellationToken ct = default) {
        var processInfo = new ProcessStartInfo(fileName, arguments) {
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        logger.LogInformation("Running command: {cmd}", $"{fileName} {arguments}");
        using var process = new Process();
        process.StartInfo = processInfo;
        process.EnableRaisingEvents = true;

        var outputTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        var errorTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        #pragma warning disable CA2254
        process.OutputDataReceived += (_, e) => {
            if (e.Data == null) {
                outputTcs.TrySetResult(true);
            } else if (e.Data.StartsWith("warn: ")) {
                logger.LogWarning(e.Data[6..]);
            } else if (e.Data.StartsWith("hint: ")) {
                logger.LogDebug(e.Data[6..]);
            } else if (e.Data.StartsWith("Example: ")) {
                logger.LogDebug(e.Data[9..]);
            }else {
                logger.LogInformation(e.Data);
            }
        };

        process.ErrorDataReceived += (_, e) => {
            if (e.Data == null) {
                errorTcs.TrySetResult(true);
            } else {
                logger.LogError(e.Data);
            }
        };
        #pragma warning restore CA2254

        if (!process.Start()) {
            logger.LogError("Failed to start process to run {cmd}", $"{fileName} {arguments}");
            throw new Exception($"Failed to start process to run {fileName} {arguments}");
        }

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        // Wait for process exit and the output/error streams to close
        await Task.WhenAll(
            process.WaitForExitAsync(ct),
            outputTcs.Task,
            errorTcs.Task
        );

        if (process.ExitCode != 0) {
            throw new Exception($"Command failed with exit code {process.ExitCode}: {fileName} {arguments}");
        }
    }
}
