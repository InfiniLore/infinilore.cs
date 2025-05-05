// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Kiota.Models;
using InfiniLore.Shared;
using InfiniLore.Shared.Modules.MarkdownFiles.Database;
using InfiniLore.Shared.Modules.MarkdownFiles.Services;
using InfiniLore.Wasm.Contracts.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Serialization;
using System.Text.Json;

namespace InfiniLore.Wasm.Modules.MarkdownFiles.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IMarkdownFileInteractiveApi>]
public class MarkdownFileInteractiveApi(
    ILogger<MarkdownFileInteractiveApi> logger,
    IInteractiveApiWasm interactiveApi
) : IMarkdownFileInteractiveApi {
    
    public async ValueTask<Result<PaginatedData<IMarkdownFileModel>>> GetMarkdownFilesAsync(string loreScopeId, CancellationToken ct = default) {
        try {
            var client = interactiveApi.ApiClient;
            var requestBuilder = client.Api.V1.Data.Project[loreScopeId].MarkdownFile;
            var result = await requestBuilder
                .GetAsync(cancellationToken: ct);
            
            if (result is null) return Result<PaginatedData<IMarkdownFileModel>>.FromError("Could not get data from API");
            Stream jsonStream = result.SerializeAsJsonStream();
            var response = await JsonSerializer.DeserializeAsync<PaginatedData<IMarkdownFileModel>>(jsonStream, interactiveApi.JsonOptions, ct);
            return Result<PaginatedData<IMarkdownFileModel>>.FromSuccess(response);
        }
        catch (Exception e) {
            logger.Error(e, "Failed to get MarkdownFiles for loreScopeId {loreScopeId} because '{reason}'", loreScopeId, e.Message);
            return Result<PaginatedData<IMarkdownFileModel>>.FromError($"Unknown failure");
        }
    }
    
    public async ValueTask<Result<IMarkdownFileModel>> GetMarkdownFileAsync(string loreScopeId, string markdownFileId, CancellationToken ct = default) {
        try {
            var client = interactiveApi.ApiClient;
            var requestBuilder = client.Api.V1.Data.Project[loreScopeId].MarkdownFile[markdownFileId];
            var result = await requestBuilder
                .GetAsync(cancellationToken: ct);
            
            if (result is null) return Result<IMarkdownFileModel>.FromError("Could not get data from API");
            Stream jsonStream = result.SerializeAsJsonStream();
            var response = await JsonSerializer.DeserializeAsync<IMarkdownFileModel>(jsonStream, interactiveApi.JsonOptions, ct);
            return Result<IMarkdownFileModel>.FromSuccess(response!);
        }
        catch (Exception e) {
            logger.Error(e, "Failed to get MarkdownFile for loreScopeId {loreScopeId} because '{reason}'", loreScopeId, e.Message);
            return Result<IMarkdownFileModel>.FromError("Unknown failure");
        }
    }
    
    public async ValueTask<Result> UpsertMarkdownFileAsync(string loreScopeId, string markdownFileId, string fileName, string markdown, CancellationToken ct = default) {
        try {
            var client = interactiveApi.ApiClient;
            var requestBuilder = client.Api.V1.Data.Project[loreScopeId].MarkdownFile[markdownFileId];
            var requestBody = new InfiniLoreServerApiEndpointsDataProjectMarkdownFilesUpsertMarkdownFileUpsertMarkdownFileRequest() {
                FileName = fileName,
                Source = markdown
            };
            await requestBuilder.PostAsync(requestBody, cancellationToken: ct);
            return true;
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }    
    }
}
