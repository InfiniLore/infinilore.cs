// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Shared;
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
): CommandHandler<GetLorescopePosterImageQuery, Outcome<string>> {

    public override async Task<Outcome<string>> ExecuteAsync(GetLorescopePosterImageQuery command, CancellationToken ct = new()) {
        await using IUnitOfWork unitOfWork = readonlyUnitOfWorkFactory.Create();
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        RepoOutcome<LoreScopeModel> foundModelResult = await loreScopeRepo.GetByIdAsync(command.LorescopeId, ct: ct);
        if (!foundModelResult.TryGetAsData(out LoreScopeModel? foundModel)) {
            return Outcome<string>.FromError($"Failed to find lorescope with id {command.LorescopeId}");
        }

        if (foundModel.PosterImageMetaData == null || foundModel.PosterImageMetaDataId == null) {
            return Outcome<string>.FromError("No poster image found for this lorescope");
        }

        Outcome<string> result = await s3FileStorage.GetFileUrlAsync(
            S3BucketNames.GetLoreScopeBucket(foundModel.Id),
            foundModel.PosterImageMetaData.FileName,
            ct: ct
        );
        return result.Match(
            Outcome.FromData,
            _ => Outcome<string>.FromError("Failed to get poster image url")
        );
    }
}
