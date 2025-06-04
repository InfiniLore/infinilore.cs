// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;
using InfiniLore.Server.Modules.LsMarkdownFiles.Messaging.Queries;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.Messaging.Queries;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetLsMarkdownFileByIdHandler(
    IReadonlyUnitOfWorkFactory factory,
    IS3FileStorage fileStorage,
    ILogger<GetLsMarkdownFileByIdHandler> logger
) : AccessProtectedCommandHandler<GetLsMarkdownFileByIdQuery, LsMarkdownFileModel>(logger) {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<Outcome<LsMarkdownFileModel>> HandleCommandAsync(GetLsMarkdownFileByIdQuery command, CancellationToken ct = default) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var markdownFileRepository = await unitOfWork.GetRepositoryAsync<ILsMarkdownFileRepository>(ct);

        RepoOutcome<LsMarkdownFileModel> response = await markdownFileRepository.GetByIdAsync(command.FileId, command.QueryConfig, ct);
        if (!response.TryGetAsData(out LsMarkdownFileModel? model)) return Outcome<LsMarkdownFileModel>.FromError("Failed to get markdown file");
        if (model.S3FileMetaData is null) return Outcome<LsMarkdownFileModel>.FromError("Failed to get complete S3FileMetaData");
        
        // Get the url for the file
        string bucketName =  S3BucketNames.GetLoreScopeBucket(model.OwnerId);
        Outcome<string> urlResult = await fileStorage.GetFileUrlAsync(bucketName, model.S3FileMetaData.FileName, ct:ct);
        urlResult.Switch(
            url => model.S3FileMetaData.S3ResourceUrl = url,
            _ => logger.Warning("Failed to get s3 resource url"), 
            _ => logger.Warning("Failed to get s3 resource url")
        );
        
        return model;
    }

    protected override ValueTask<bool> ValidateAccessAsync(GetLsMarkdownFileByIdQuery command, CancellationToken ct = default) {
        return ValueTask.FromResult(true); // TODO fix this so only the correct people can access this.
    }
}