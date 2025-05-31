// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LoreScopes.Messaging.Queries;
using JetBrains.Annotations;

namespace InfiniLore.Modules.LoreScopes.Server.Messaging.Queries;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetLorescopePosterImageHandler(
    IReadonlyUnitOfWorkFactory readonlyUnitOfWorkFactory,
    IS3FileStorage s3FileStorage
): CommandHandler<GetLorescopePosterImageQuery, MessageResponse<string>> {

    public override async Task<MessageResponse<string>> ExecuteAsync(GetLorescopePosterImageQuery command, CancellationToken ct = new()) {
        await using IUnitOfWork unitOfWork = readonlyUnitOfWorkFactory.Create();
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);
    
        Result<LoreScopeModel> foundModelResult = await loreScopeRepo.GetByIdAsync(command.LorescopeId, ct: ct);
        if (!foundModelResult.TryGetAsSuccess(out LoreScopeModel? foundModel)) {
            return MessageResponse.FromErrorString($"Failed to find lorescope with id {command.LorescopeId}");
        }

        if (foundModel.PosterImageMetaData == null || foundModel.PosterImageMetaDataId == null) {
            return MessageResponse.FromErrorString("No poster image found for this lorescope");
        }

        Result<string> result = await s3FileStorage.GetFileUrlAsync(foundModel.S3BucketName, foundModel.PosterImageMetaData.FileName, ct:ct);
        return result.Match(
            MessageResponse.FromSuccess,
            _ => MessageResponse.FromErrorString("Failed to get poster image url")
        );
    }
}
