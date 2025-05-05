// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.MarkdownFiles.Database;
using InfiniLore.Server.Modules.MarkdownFiles.DataBase;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.MarkdownFiles.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetMarkdownFilesHandler(
    IReadonlyUnitOfWorkFactory factory, 
    ILogger<GetMarkdownFilesHandler> logger
) : CommandHandler<GetMarkdownFilesQuery, MessageResponse<PaginatedData<MarkdownFileModel>>> {

    public override async Task<MessageResponse<PaginatedData<MarkdownFileModel>>> ExecuteAsync(GetMarkdownFilesQuery command, CancellationToken ct = new()) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var markdownFileRepository = await unitOfWork.GetRepositoryAsync<IMarkdownFileRepository>(ct);

        var queryConfig = new QueryConfig(command.AutoInclude, command.Reverse);
        PaginatedResult<MarkdownFileModel> response = await markdownFileRepository.GetByOwnerAsync(command.LorescopeId, command.PaginationInfo, queryConfig, ct);
        
        // ReSharper disable once InvertIf
        if (!response.TryGetAsSuccess(out PaginatedData<MarkdownFileModel> paginatedResult)) {
            logger.Warning("Failed to get MarkdownFiles");
            return MessageResponse<PaginatedData<MarkdownFileModel>>.FromErrorString("Failed to get MarkdownFiles");
        }

        return MessageResponse<PaginatedData<MarkdownFileModel>>.FromSuccess(paginatedResult);
    }
}
