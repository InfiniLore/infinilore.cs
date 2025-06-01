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
            KiotaLsMarkdownFilesResponse? _ = await requestBuilder.GetAsync(parameters => {} );
        }
        catch (Exception e) {
            logger.Error(e, "Could not get ls markdown files");
            return PaginatedResult<ILsMarkdownFileModel>.FromError("Could not get ls markdown files");
        }
        
        throw new NotImplementedException();
    }
    
    public ValueTask<Result> UpsertLsMarkdownFileAsync(string loreScopeId, string fileName, Stream fileData, CancellationToken ct = default) 
        => throw new NotImplementedException();
    
    public ValueTask<Result> DeleteLsMarkdownFileAsync(string loreScopeId, string lsMarkdownFileId, CancellationToken ct = default) 
        => throw new NotImplementedException();
}
