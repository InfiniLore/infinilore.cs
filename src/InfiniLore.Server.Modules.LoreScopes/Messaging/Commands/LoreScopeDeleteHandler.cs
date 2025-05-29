// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Modules.Core;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.Core.Messaging.Handlers;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Shared.Auth;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.LoreScopes.Messaging.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class LoreScopeDeleteHandler(
    IUnitOfWorkFactory unitOfWorkFactory,
    IAccessProtectionRules accessProtectionRules,
    ILogger<LoreScopeDeleteHandler> logger
) : AccessProtectedCommandHandler<DeleteLoreScopeRequest>(logger) {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<MessageResponse> HandleCommandAsync(DeleteLoreScopeRequest command, CancellationToken ct = default) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        Result result = await loreScopeRepo.DeleteByIdAsync(command.LoreScopeId, ct);
        if (!result.IsError) return true;

        logger.Warning("Failed to delete lorescope at id {id}", command.LoreScopeId);
        return MessageResponse.FromErrorString("Failed to delete lorescope");
    }

    protected override ValueTask<bool> ValidateAccessAsync(DeleteLoreScopeRequest command, CancellationToken ct = default) 
        => accessProtectionRules.CanAccessRepoWithPermission<ILoreScopeRepository>(
            command.LoreScopeId, 
            command.AccessingUser,
            PermissionsStore.LorescopeDelete,
            ct
        );
}
