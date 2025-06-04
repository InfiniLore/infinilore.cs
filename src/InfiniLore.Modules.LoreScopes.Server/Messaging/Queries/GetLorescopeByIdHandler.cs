// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LoreScopes.Messaging.Queries;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.LoreScopes.Server.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetLorescopeByIdHandler(
    IReadonlyUnitOfWorkFactory factory,
    IAccessProtectionRules protectionRules,
    ILogger<GetLorescopeByIdHandler> logger
) : AccessProtectedCommandHandler<GetLorescopeByIdQuery, LoreScopeModel>(logger) {
    protected override Outcome<LoreScopeModel> AccessDeniedOutcome => Outcome.FromError("Access denied");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<Outcome<LoreScopeModel>> HandleCommandAsync(GetLorescopeByIdQuery command, CancellationToken ct = default) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var loreScopeRepository = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        var queryConfig = new QueryConfig(OptionalInclude: command.AutoInclude);
        RepoOutcome<LoreScopeModel> response = await loreScopeRepository.GetByIdAsync(command.LorescopeId, queryConfig, ct);

        if (!response.TryGetAsData(out LoreScopeModel? value)) {
            logger.Warning("Failed to get lorescope");
            return Outcome.FromError("Failed to get lorescope");
        }

        // ReSharper disable once InvertIf
        if (!command.IsLoreScopeOnly && value.OwnerId != command.UserId) {
            logger.Warning("User does not own this lorescope");
            return Outcome.FromError("User does not own this lorescope");
        }

        return Outcome.FromData(value);
    } 
    
    protected override ValueTask<bool> ValidateAccessAsync(GetLorescopeByIdQuery command, CancellationToken ct = default)
        => protectionRules.CanAccessRepoWithPermission<ILoreScopeRepository>(
            command.LorescopeId, 
            command.AccessingUser,
            PermissionsStore.LorescopeRead,
            ct
        );
}
