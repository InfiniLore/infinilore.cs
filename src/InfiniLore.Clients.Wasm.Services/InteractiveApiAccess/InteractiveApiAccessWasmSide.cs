// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Clients.Kiota;
using InfiniLore.Clients.Kiota.Models;
using InfiniLore.Shared.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Serialization;
using System.Text.Json;

namespace InfiniLore.Clients.Wasm.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IInteractiveApiAccess>(ServiceLifetime.Scoped)]
public class InteractiveApiAccessWasmSide(
    ILogger<InteractiveApiAccessWasmSide> logger,
    InfiniLoreApiClient apiClient
) : IInteractiveApiAccess {
    private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region LoreScopes
    public async ValueTask<Result<LoreScopesResponse>> GetLoreScopesAsync(string userId, CancellationToken ct = default) {
        // ReSharper disable twice SuggestVarOrType_SimpleTypes
        try {
            var requestBuilder = apiClient.Api.V1.Data.User[userId].Lorescope;
            var result = await requestBuilder
                .GetAsync(cancellationToken: ct);

            if (result is null) return Result<LoreScopesResponse>.FromError("Could not get data from API");

            Stream jsonStream = result.SerializeAsJsonStream();
            var response = await JsonSerializer.DeserializeAsync<LoreScopesResponse>(jsonStream, Options, ct);

            if (response is null) return Result<LoreScopesResponse>.FromError("Could not deserialize data from API");

            return Result<LoreScopesResponse>.FromSuccess(response);
        }
        catch (Exception e) {
            logger.Error(e, "Failed to get LoreScopes for user {userId} because '{reason}'", userId, e.Message);
            return Result<LoreScopesResponse>.FromError($"Unknown failure");
        }
    }
    #endregion

    public async ValueTask<Result<MarkdownFilesResponse>> GetMarkdownFilesAsync(string loreScopeId, CancellationToken ct = default) {
        try {
            var requestBuilder = apiClient.Api.V1.Data.Project[loreScopeId].MarkdownFile;
            var result = await requestBuilder
                .GetAsync(cancellationToken: ct);
            
            if (result is null) return Result<MarkdownFilesResponse>.FromError("Could not get data from API");
            Stream jsonStream = result.SerializeAsJsonStream();
            var response = await JsonSerializer.DeserializeAsync<MarkdownFilesResponse>(jsonStream, Options, ct);
            if (response is null) return Result<MarkdownFilesResponse>.FromError("Could not deserialize data from API");
            return Result<MarkdownFilesResponse>.FromSuccess(response);
        }
        catch (Exception e) {
            logger.Error(e, "Failed to get MarkdownFiles for loreScopeId {loreScopeId} because '{reason}'", loreScopeId, e.Message);
            return Result<MarkdownFilesResponse>.FromError($"Unknown failure");
        }
    }
    
    public async ValueTask<Result<MarkdownFileResponse>> GetMarkdownFileAsync(string loreScopeId, string markdownFileId, CancellationToken ct = default) {
        try {
            var requestBuilder = apiClient.Api.V1.Data.Project[loreScopeId].MarkdownFile[markdownFileId];
            var result = await requestBuilder
                .GetAsync(cancellationToken: ct);
            
            if (result is null) return Result<MarkdownFileResponse>.FromError("Could not get data from API");
            Stream jsonStream = result.SerializeAsJsonStream();
            var response = await JsonSerializer.DeserializeAsync<MarkdownFileResponse>(jsonStream, Options, ct);
            if (response is null) return Result<MarkdownFileResponse>.FromError("Could not deserialize data from API");
            return Result<MarkdownFileResponse>.FromSuccess(response);
        }
        catch (Exception e) {
            logger.Error(e, "Failed to get MarkdownFile for loreScopeId {loreScopeId} because '{reason}'", loreScopeId, e.Message);
            return Result<MarkdownFileResponse>.FromError("Unknown failure");
        }
    }
    
    public async ValueTask<Result> UpsertMarkdownFileAsync(string loreScopeId, string markdownFileId, string fileName, string markdown, CancellationToken ct = default) {
        try {
            var requestBuilder = apiClient.Api.V1.Data.Project[loreScopeId].MarkdownFile[markdownFileId];
            var requestBody = new InfiniLoreServerApiEndpointsDataProjectMarkdownFilesUpsertMarkdownFileUpsertMarkdownFileRequest() {
                FileName = fileName,
                Source = markdown
            };
            var result = await requestBuilder.PostAsync(requestBody, cancellationToken: ct);
            return true;
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }    
    }

}
