// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
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
public class GetMarkdownFileByIdHandler(IReadonlyUnitOfWorkFactory factory, ILogger<GetMarkdownFileByIdHandler> logger) : CommandHandler<GetMarkdownFileByIdQuery, MessageResponse<MarkdownFile>> {

    public override async Task<MessageResponse<MarkdownFile>> ExecuteAsync(GetMarkdownFileByIdQuery command, CancellationToken ct = new()) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var markdownFileRepository = await unitOfWork.GetRepositoryAsync<IMarkdownFileRepository>(ct);

        var queryConfig = new QueryConfig(AutoInclude: command.AutoInclude);
        Result<MarkdownFile> response = await markdownFileRepository.GetByIdAsync(command.LorescopeId, queryConfig, ct);

        if (!response.TryGetAsSuccess(out MarkdownFile value)) {
            logger.Warning("Failed to get lorescope");
            return MessageResponse<MarkdownFile>.FromErrorString("Failed to get lorescope");
        }

        return MessageResponse<MarkdownFile>.FromSuccess(value);
    }
}
