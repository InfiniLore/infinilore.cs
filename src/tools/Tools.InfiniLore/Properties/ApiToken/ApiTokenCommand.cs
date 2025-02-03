// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using InfiniLore.KiotaApiClient;
using InfiniLore.KiotaApiClient.Models;
using InfiniLore.Server.Services;
using JetBrains.Annotations;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using TextCopy;

namespace Tools.InfiniLore.Properties.ApiToken;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[CliArgsCommand("api-token")]
public partial class ApiTokenCommand : ICommand<ApiTokenParameters> {

    /// <summary>
    ///     Executes the process of generating an API token by sending a request using the provided parameters.
    /// </summary>
    /// <param name="parameters">
    ///     An instance of <see cref="ApiTokenParameters" /> containing the username, password, and other necessary information
    ///     for the API token request.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. Writes the generated API token to the console.
    /// </returns>
    /// <exception cref="Exception">
    ///     Throws an exception if the response data is null.
    /// </exception>
    public async Task ExecuteAsync(ApiTokenParameters parameters) {
        // Okay this is hideous, but it will have to do for now
        //      I just wanted an easy way to get the API token, and not go through swagger all the time.
        //      I'll fix this later when I use PostMan 
        var authProvider = new AnonymousAuthenticationProvider();
        var adapter = new HttpClientRequestAdapter(authProvider);
        var apiClient = new InfiniLoreApiClient(adapter);

        // ReSharper disable once SuggestVarOrType_SimpleTypes
        var body = new InfiniLoreServerAPIControllersAccountJWTCreateJwtCreateTokensRequest {
            Username = parameters.User,
            Password = parameters.Password,
            Roles = [],
            Permissions = ApiPermissionsStore.GetAllPermissions().ToList()
        };

        InfiniLoreServerAPIModelsJwtResponse? data = await apiClient.Api.Account.Jwt.TokensCreate.PostAsync(
            body
        );

        if (data is null) throw new Exception("No data returned");

        Console.WriteLine("Token generated successfully.");
        Console.WriteLine();

        Console.WriteLine($"'{parameters.User}' has userId :");
        // Console.WriteLine(data.OwnerId);
        Console.WriteLine();

        Console.WriteLine($"'{parameters.User}' has refresh token :");
        Console.WriteLine(data.RefreshToken);
        Console.WriteLine();

        Console.WriteLine($"AccessToken for '{parameters.User}'");
        Console.WriteLine(data.AccessToken);

        string? toCopy = parameters.Copy switch {
            SectionToCopy.ApiToken => data.AccessToken,
            SectionToCopy.JwtRefreshToken => data.RefreshToken,
            SectionToCopy.None => null,
            _ => null
        };

        if (toCopy.IsNullOrWhiteSpace()) return;

        try {
            await ClipboardService.SetTextAsync(toCopy);
            Console.WriteLine($"'{parameters.Copy}' has been copied to the clipboard.");
        }
        catch (Exception ex) {
            Console.WriteLine($"Failed to copy the token to the clipboard: {ex.Message}");
        }

    }
}
