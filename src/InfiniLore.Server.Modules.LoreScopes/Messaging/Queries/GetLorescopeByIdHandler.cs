// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Shared.Auth;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.LoreScopes.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetLorescopeByIdHandler(
    IReadonlyUnitOfWorkFactory factory,
    ILogger<GetLorescopeByIdHandler> logger
) : AccessRestrictedCommandHandler<GetLorescopeByIdQuery, MessageResponse<LoreScopeModel>>(logger) {
    protected override MessageResponse<LoreScopeModel> AccessDeniedResult => MessageResponse.FromErrorString("Access denied");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<MessageResponse<LoreScopeModel>> HandleCommandAsync(GetLorescopeByIdQuery command, CancellationToken ct = default) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var loreScopeRepository = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        var queryConfig = new QueryConfig(OptionalInclude: command.AutoInclude);
        Result<LoreScopeModel> response = await loreScopeRepository.GetByIdAsync(command.LorescopeId, queryConfig, ct);

        if (!response.TryGetAsSuccess(out LoreScopeModel value)) {
            logger.Warning("Failed to get lorescope");
            return MessageResponse.FromErrorString("Failed to get lorescope");
        }

        // ReSharper disable once InvertIf
        if (!command.IsLoreScopeOnly && value.OwnerId != command.UserId) {
            logger.Warning("User does not own this lorescope");
            return MessageResponse.FromErrorString("User does not own this lorescope");
        }

        return MessageResponse.FromSuccess(value);
    } 
    
    protected override async ValueTask<bool> ValidateAccessAsync(GetLorescopeByIdQuery command, CancellationToken ct = default) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var loreScopeRepository = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);
        var accessRepo = await unitOfWork.GetRepositoryAsync<IAccessProtectionRepository>(ct);
        
        Result<LoreScopeModel> response = await loreScopeRepository.GetByIdAsync(command.LorescopeId, ct:ct);
        if (!response.TryGetAsSuccess(out LoreScopeModel loreScope)) {
            logger.Warning("Failed to get lorescope");
            return false;
        }

        if (!loreScope.HasAccessProtection) {
            logger.Warning("LoreScope does not have access protection");
            return false;
        }
        
        Result<AccessProtectionModel> accessResult = await accessRepo.GetByIdAsync((Guid)loreScope.AccessProtectionId, ct:ct);
        if (!accessResult.TryGetAsSuccess(out AccessProtectionModel accessProtection)) {
            logger.Warning("Failed to get access protection");
            return false;
        }

        return accessProtection.HasPermission(command.Access.UserId, PermissionsStore.LorescopeRead);
    }
}
