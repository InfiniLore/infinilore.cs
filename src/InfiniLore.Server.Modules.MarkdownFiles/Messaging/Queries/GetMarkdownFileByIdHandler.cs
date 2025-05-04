// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Server.Database.Models;
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
public class GetMarkdownFileByIdHandler(IReadonlyUnitOfWorkFactory factory, ILogger<GetMarkdownFileByIdHandler> logger) : CommandHandler<GetMarkdownFileByIdQuery, MessageResponse<MarkdownFile>> {

    public override async Task<MessageResponse<MarkdownFile>> ExecuteAsync(GetMarkdownFileByIdQuery command, CancellationToken ct = new()) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var markdownFileRepository = await unitOfWork.GetRepositoryAsync<IMarkdownFileRepository>(ct);

        var queryConfig = new QueryConfig(AutoInclude: command.AutoInclude);
        Result<MarkdownFile> response = await markdownFileRepository.GetByIdAsync(command.MarkdownFileId, queryConfig, ct);

        if (!response.TryGetAsSuccess(out MarkdownFile value)) {
            logger.Warning("Failed to get lorescope");
            return MessageResponse<MarkdownFile>.FromErrorString("Failed to get lorescope");
        }

        return MessageResponse<MarkdownFile>.FromSuccess(value);
    }
}
