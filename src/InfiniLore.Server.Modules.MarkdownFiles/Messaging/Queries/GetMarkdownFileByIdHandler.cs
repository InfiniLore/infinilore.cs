// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Server.Modules.Core.Database.Models;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.MarkdownFiles.DataBase;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.MarkdownFiles.Messaging.Queries;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetMarkdownFileByIdHandler(IReadonlyUnitOfWorkFactory factory, ILogger<GetMarkdownFileByIdHandler> logger) : CommandHandler<GetMarkdownFileByIdQuery, MessageResponse<IMarkdownFile>> {

    public override async Task<MessageResponse<IMarkdownFile>> ExecuteAsync(GetMarkdownFileByIdQuery command, CancellationToken ct = new()) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var markdownFileRepository = await unitOfWork.GetRepositoryAsync<IMarkdownFileRepository>(ct);

        var queryConfig = new QueryConfig(AutoInclude: command.AutoInclude);
        Result<IMarkdownFile> response = await markdownFileRepository.GetByIdAsync(command.MarkdownFileId, queryConfig, ct);

        if (!response.TryGetAsSuccess(out IMarkdownFile value)) {
            logger.Warning("Failed to get lorescope");
            return MessageResponse<IMarkdownFile>.FromErrorString("Failed to get lorescope");
        }

        return MessageResponse<IMarkdownFile>.FromSuccess(value);
    }
}
