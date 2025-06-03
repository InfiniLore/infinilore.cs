// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LoreScopes.Messaging.Queries;
using JetBrains.Annotations;
using Result=InfiniLore.Modules.Core.Server.Result;

namespace InfiniLore.Modules.LoreScopes.Server.Messaging.Queries;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetLorescopePosterImageHandler(
    IReadonlyUnitOfWorkFactory readonlyUnitOfWorkFactory,
    IS3FileStorage s3FileStorage
): CommandHandler<GetLorescopePosterImageQuery, Core.Server.Result<string>> {

    public override async Task<Core.Server.Result<string>> ExecuteAsync(GetLorescopePosterImageQuery command, CancellationToken ct = new()) {
        await using IUnitOfWork unitOfWork = readonlyUnitOfWorkFactory.Create();
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        AterraEngine.Unions.Result<LoreScopeModel> foundModelResult = await loreScopeRepo.GetByIdAsync(command.LorescopeId, ct: ct);
        if (!foundModelResult.TryGetAsSuccess(out LoreScopeModel? foundModel)) {
            return Result.FromError($"Failed to find lorescope with id {command.LorescopeId}");
        }

        if (foundModel.PosterImageMetaData == null || foundModel.PosterImageMetaDataId == null) {
            return Result.FromError("No poster image found for this lorescope");
        }

        AterraEngine.Unions.Result<string> result = await s3FileStorage.GetFileUrlAsync(
            S3BucketNames.GetLoreScopeBucket(foundModel.Id),
            foundModel.PosterImageMetaData.FileName,
            ct: ct
        );
        return result.Match(
            Result.FromSuccess,
            _ => Result.FromError("Failed to get poster image url")
        );
    }
}
