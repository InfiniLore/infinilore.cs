// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Kiota;
using InfiniLore.Kiota.Api.V1.DataLorescope.Item.MarkdownFile;
using InfiniLore.Kiota.Api.V1.DataLorescope.Item.MarkdownFile.Item;
using InfiniLore.Kiota.Models;
using InfiniLore.Modules.Core.Wasm.Contracts.Services;
using InfiniLore.Modules.LsMarkdownFiles.Shared.Database;
using InfiniLore.Modules.LsMarkdownFiles.Shared.Services;
using InfiniLore.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;

namespace InfiniLore.Modules.LsMarkdownFiles.Wasm.Services.InteractiveApi;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInteractiveApiLsMarkdownFiles>]
public class InteractiveApiWasmLsMarkdownFile(
    ILogger<InteractiveApiWasmLsMarkdownFile> logger,
    IInteractiveApiWasm interactiveApi
) : IInteractiveApiLsMarkdownFiles {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<Result<ILsMarkdownFileModel>> GetLsMarkdownFileAsync(string loreScopeId, string lsMarkdownFileId, CancellationToken ct = default) {
        try {
            InfiniLoreApiClient client = interactiveApi.ApiClient;
            WithMarkdownFileItemRequestBuilder requestBuilder = client.Api.V1.DataLorescope[loreScopeId].MarkdownFile[lsMarkdownFileId];
            KiotaLsMarkdownFileResponse? result = await requestBuilder.GetAsync(cancellationToken: ct);

            if (result is null) return Result<ILsMarkdownFileModel>.FromError(interactiveApi.DefaultApiError);

            return Result<ILsMarkdownFileModel>.FromSuccess(WasmLsMarkdownFileModel.FromKiotaModel(result));
        }
        catch (Exception e) {
            logger.Error(e, "Could not get ls markdown file by id {id}", lsMarkdownFileId);
            return Result<ILsMarkdownFileModel>.FromError("Could not get ls markdown file by id");
        }
    }

    public async ValueTask<PaginatedResult<ILsMarkdownFileModel>> GetLsMarkdownFilesAsync(string loreScopeId, PaginationInfo pagination, CancellationToken ct = default) {
        try {
            InfiniLoreApiClient client = interactiveApi.ApiClient;
            MarkdownFileRequestBuilder requestBuilder = client.Api.V1.DataLorescope[loreScopeId].MarkdownFile;

            KiotaLsMarkdownFilesResponse? result = await requestBuilder.GetAsync(requestConfiguration: config => {
                    config.QueryParameters.PageNumber = pagination.PageNumber;
                    config.QueryParameters.Reverse = false;
                },
                ct
            );

            if (result is null) return PaginatedResult<ILsMarkdownFileModel>.FromError("Could not get data from API");
            
            ILsMarkdownFileModel[] items = (result.Items ?? Enumerable.Empty<KiotaLsMarkdownFileResponse>())
                .Select(WasmLsMarkdownFileModel.FromKiotaModel)
                .ToArray();

            return new PaginatedData<ILsMarkdownFileModel>(
                items,
                result.TotalCount ?? items.Length,
                result.CurrentPage ?? 1,// Default to page 1 if null
                result.TotalPages ?? 1// Default to 1 page if null
            );
        }
        catch (Exception e) {
            logger.Error(e, "Could not get ls markdown files");
            return PaginatedResult<ILsMarkdownFileModel>.FromError("Could not get ls markdown files");
        }
    }

    public async ValueTask<Result> UpsertLsMarkdownFileAsync(string loreScopeId, string fileName, Stream fileData, CancellationToken ct = default) {
        try {
            InfiniLoreApiClient client = interactiveApi.ApiClient;
            MarkdownFileRequestBuilder requestBuilder = client.Api.V1.DataLorescope[loreScopeId].MarkdownFile;

            // Read the stream into a memory stream first
            using var memoryStream = new MemoryStream();
            await fileData.CopyToAsync(memoryStream, ct);
            memoryStream.Position = 0;// Reset position to beginning

            var multipartBody = new MultipartBody();
            multipartBody.AddOrReplacePart(
                "File",// This must match exactly with the server-side model property name
                "text/markdown",
                memoryStream,
                fileName
            );

            Stream? result = await requestBuilder.PostAsync(multipartBody, cancellationToken: ct);
            if (result is null) return Result.FromError(interactiveApi.DefaultApiError);

            return Result.FromState(true);
        }
        catch (Exception e) {
            logger.Error(e, "Could not upsert ls markdown file");
            return Result.FromError("Could not upsert ls markdown file");
        }
    }

    public async ValueTask<Result> DeleteLsMarkdownFileAsync(string loreScopeId, string lsMarkdownFileId, CancellationToken ct = default) {
        try {
            InfiniLoreApiClient client = interactiveApi.ApiClient;
            WithMarkdownFileItemRequestBuilder requestBuilder = client.Api.V1.DataLorescope[loreScopeId].MarkdownFile[lsMarkdownFileId];

            Stream? result = await requestBuilder.DeleteAsync(cancellationToken: ct);

            if (result is null) return Result.FromError(interactiveApi.DefaultApiError);

            return Result.FromState(true);
        }
        catch (Exception e) {
            logger.Error(e, "Could not delete ls markdown file");
            return Result.FromError("Could not delete ls markdown file");
        }
    }
}
