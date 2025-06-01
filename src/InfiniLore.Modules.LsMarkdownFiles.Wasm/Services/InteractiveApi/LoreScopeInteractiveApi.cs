// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.LsMarkdownFiles.Shared.Database;
using InfiniLore.Modules.LsMarkdownFiles.Shared.Services;
using InfiniLore.Shared;

namespace InfiniLore.Modules.LsMarkdownFiles.Wasm.Services.InteractiveApi;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInteractiveApiLsMarkdownFiles>]
public class LsMarkdownFileInteractiveApi : IInteractiveApiLsMarkdownFiles {
    
    public ValueTask<Result<ILsMarkdownFileModel>> GetLsMarkdownFileAsync(string loreScopeId, string lsMarkdownFileId, CancellationToken ct = default) 
        => throw new NotImplementedException();
    public ValueTask<PaginatedResult<ILsMarkdownFileModel>> GetLsMarkdownFilesAsync(string loreScopeId, PaginationInfo pagination,CancellationToken ct = default) 
        => throw new NotImplementedException();
    public ValueTask<Result> UpsertLsMarkdownFileAsync(string loreScopeId, string fileName, Stream fileData, CancellationToken ct = default) 
        => throw new NotImplementedException();
    public ValueTask<Result> DeleteLsMarkdownFileAsync(string loreScopeId, string lsMarkdownFileId, CancellationToken ct = default) 
        => throw new NotImplementedException();
}
