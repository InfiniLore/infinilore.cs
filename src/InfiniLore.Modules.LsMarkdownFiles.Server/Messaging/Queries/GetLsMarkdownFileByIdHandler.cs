// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
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
    protected override MessageResponse<LsMarkdownFileModel> AccessDeniedResult => MessageResponse.FromErrorString("Access denied");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<MessageResponse<LsMarkdownFileModel>> HandleCommandAsync(GetLsMarkdownFileByIdQuery command, CancellationToken ct = default) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var markdownFileRepository = await unitOfWork.GetRepositoryAsync<ILsMarkdownFileRepository>(ct);
        
        Result<LsMarkdownFileModel> response = await markdownFileRepository.GetByIdAsync(command.FileId, command.QueryConfig, ct);
        if (!response.TryGetAsSuccess(out LsMarkdownFileModel? model)) return MessageResponse.FromErrorString("Failed to get markdown file");
        if (model.S3FileMetaData is null) return MessageResponse.FromErrorString("Failed to get complete S3FileMetaData");
        
        string bucketName = model.GetLoreScopeBucketName();
        Result<string> urlResult = await fileStorage.GetFileUrlAsync(bucketName, model.S3FileMetaData.FileName, ct:ct);
        urlResult.Switch(
            url => model.S3FileMetaData.S3ResourceUrl = url,
            _ => logger.Warning("Failed to get s3 resource url")
        );
        
        return model;
    }

    protected override ValueTask<bool> ValidateAccessAsync(GetLsMarkdownFileByIdQuery command, CancellationToken ct = default) {
        return ValueTask.FromResult(true); // TODO fix this so only the correct people can access this.
    }
}