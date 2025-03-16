// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Credentials.Auth0;
using InfiniLore.Credentials.Auth0.OptionConfiguration;
using InfiniLore.Credentials.Auth0.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Tools.InfiniLore.Auth0.Setup;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class NetUserSecretAuth0AccessTokenStore(IOptions<Auth0Options> options, ILogger<NetUserSecretAuth0AccessTokenStore> logger) : IAuth0AccessTokenStore {
    private const string AccessTokenKey = "Auth0:AccessToken";// Key to use when storing in user-secrets

    private IAuth0AccessToken AccessToken { get; set; } = TryGetFromOptions(options)
        ?? Auth0AccessToken.Empty;

    public ValueTask<IAuth0AccessToken> GetAccessTokenAsync(CancellationToken ct = default) => ValueTask.FromResult(AccessToken);

    public async ValueTask SetAccessTokenAsync(IAuth0AccessToken token, CancellationToken ct = default) {
        AccessToken = token;

        // Persist the token using the User Secrets CLI
        await SetTokenInUserSecretsAsync(token.Token);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static Auth0AccessToken? TryGetFromOptions(IOptions<Auth0Options> options) {
        if (options.Value.AccessToken is null || options.Value.AccessToken.IsNullOrWhiteSpace()) return null;

        return Auth0AccessToken.FromString(options.Value.AccessToken);
    }

    private async Task SetTokenInUserSecretsAsync(string tokenValue) {
        try {
            // Set the access token using the CLI
            await ExecuteDotNetUserSecretsCommandAsync($"set \"{AccessTokenKey}\" \"{tokenValue}\"");
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to set the access token in user-secrets.");
        }
    }

    private async Task ExecuteDotNetUserSecretsCommandAsync(string arguments) {
        var processStartInfo = new ProcessStartInfo {
            FileName = "dotnet",
            Arguments = $"user-secrets {arguments}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = @"C:\Data\Dev\infinilore.cs\tools\Tools.InfiniLore.Auth0" // Path to the directory containing the .csproj file
        };

        using Process? process = Process.Start(processStartInfo);
        if (process is null) {
            logger.LogError("Failed to start process to run dotnet user-secrets.");
            return;
        }
        await process.WaitForExitAsync();

        if (process.ExitCode != 0) {
            string error = await process.StandardError.ReadToEndAsync();
            logger.LogError(error);
        }
    }
}
