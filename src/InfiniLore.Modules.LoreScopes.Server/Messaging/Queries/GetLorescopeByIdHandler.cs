// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LoreScopes.Messaging.Queries;
using InfiniLore.Shared.Auth;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Result=InfiniLore.Modules.Core.Server.Result;

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
    protected override Core.Server.Result<LoreScopeModel> AccessDeniedResult => Result.FromError("Access denied");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<Core.Server.Result<LoreScopeModel>> HandleCommandAsync(GetLorescopeByIdQuery command, CancellationToken ct = default) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var loreScopeRepository = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        var queryConfig = new QueryConfig(OptionalInclude: command.AutoInclude);
        AterraEngine.Unions.Result<LoreScopeModel> response = await loreScopeRepository.GetByIdAsync(command.LorescopeId, queryConfig, ct);

        if (!response.TryGetAsSuccess(out LoreScopeModel? value)) {
            logger.Warning("Failed to get lorescope");
            return Result.FromError("Failed to get lorescope");
        }

        // ReSharper disable once InvertIf
        if (!command.IsLoreScopeOnly && value.OwnerId != command.UserId) {
            logger.Warning("User does not own this lorescope");
            return Result.FromError("User does not own this lorescope");
        }

        return Result.FromSuccess(value);
    } 
    
    protected override ValueTask<bool> ValidateAccessAsync(GetLorescopeByIdQuery command, CancellationToken ct = default)
        => protectionRules.CanAccessRepoWithPermission<ILoreScopeRepository>(
            command.LorescopeId, 
            command.AccessingUser,
            PermissionsStore.LorescopeRead,
            ct
        );
}
