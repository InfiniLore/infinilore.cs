// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Contracts.Database.Repositories.Data.Project;
using InfiniLore.Server.Database.Models.Data.Project;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.Messaging.Queries.Data.Project;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetMarkdownFilesHandler(
    IReadonlyUnitOfWorkFactory factory, 
    ILogger<GetMarkdownFilesHandler> logger
) : CommandHandler<GetMarkdownFilesQuery, MessageResponse<PaginatedData<MarkdownFile>>> {

    public override async Task<MessageResponse<PaginatedData<MarkdownFile>>> ExecuteAsync(GetMarkdownFilesQuery command, CancellationToken ct = new()) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var markdownFileRepository = await unitOfWork.GetRepositoryAsync<IMarkdownFileRepository>(ct);

        var queryConfig = new QueryConfig(command.AutoInclude, command.Reverse);
        PaginatedResult<MarkdownFile> response = await markdownFileRepository.GetByLoreScopeAsync(command.LorescopeId, command.PaginationInfo, queryConfig, ct);
        
        // ReSharper disable once InvertIf
        if (!response.TryGetAsSuccess(out PaginatedData<MarkdownFile> paginatedResult)) {
            logger.Warning("Failed to get MarkdownFiles");
            return MessageResponse<PaginatedData<MarkdownFile>>.FromErrorString("Failed to get MarkdownFiles");
        }

        return MessageResponse<PaginatedData<MarkdownFile>>.FromSuccess(paginatedResult);
    }
}
