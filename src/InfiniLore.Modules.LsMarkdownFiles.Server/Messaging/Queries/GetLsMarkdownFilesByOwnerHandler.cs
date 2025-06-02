// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;
using InfiniLore.Server.Modules.LsMarkdownFiles.Messaging.Queries;
using InfiniLore.Shared;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.Messaging.Queries;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetLsMarkdownFilesByOwnerHandler(
    IReadonlyUnitOfWorkFactory factory,
    IS3FileStorage fileStorage,
    ILogger<GetLsMarkdownFilesByOwnerHandler> logger
) : AccessProtectedCommandHandler<GetLsMarkdownFilesByOwnerQuery, PaginatedData<LsMarkdownFileModel>>(logger) {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<MessageResponse<PaginatedData<LsMarkdownFileModel>>> HandleCommandAsync(GetLsMarkdownFilesByOwnerQuery command, CancellationToken ct = default) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var markdownFileRepository = await unitOfWork.GetRepositoryAsync<ILsMarkdownFileRepository>(ct);
        
        PaginatedResult<LsMarkdownFileModel> response = await markdownFileRepository.GetByOwnerAsync(command.OwnerId, command.PaginationInfo, command.QueryConfig, ct);
        if (!response.TryGetAsData(out PaginatedData<LsMarkdownFileModel>? data)) return MessageResponse.FromErrorString("Failed to get markdown file");

        IEnumerable<Task> tasks = data.Value.Items.Select(async model => {
            if (model.S3FileMetaData is null) return;
            string bucketName = S3BucketNames.GetLoreScopeBucket(model.OwnerId);
            Result<string> urlResult = await fileStorage.GetFileUrlAsync(bucketName, model.S3FileMetaData.FileName, ct:ct);
            urlResult.Switch(
                url => model.S3FileMetaData.S3ResourceUrl = url,
                _ => logger.Warning("Failed to get s3 resource url for file {FileName} in bucket {BucketName}", model.S3FileMetaData.FileName, bucketName)
            );
        });
        
        await Task.WhenAll(tasks);
        return data;
    }

    protected override ValueTask<bool> ValidateAccessAsync(GetLsMarkdownFilesByOwnerQuery command, CancellationToken ct = default) {
        return ValueTask.FromResult(true); // TODO fix this so only the correct people can access this.
    }
}