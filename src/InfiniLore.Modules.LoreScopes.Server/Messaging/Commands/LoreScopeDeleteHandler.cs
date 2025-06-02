// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LoreScopes.Messaging.Commands;
using InfiniLore.Shared.Auth;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.LoreScopes.Server.Messaging.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class LoreScopeDeleteHandler(
    IUnitOfWorkFactory unitOfWorkFactory,
    IAccessProtectionRules protectionRules,
    [FromKeyedServices(IMessageBroker.FromServer)] IMessageBroker messageBroker,
    ILogger<LoreScopeDeleteHandler> logger
) : AccessProtectedCommandHandler<DeleteLoreScopeRequest>(logger) {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<MessageResponse> HandleCommandAsync(DeleteLoreScopeRequest command, CancellationToken ct = default) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        Result result = await loreScopeRepo.DeleteByIdAsync(command.LoreScopeId, ct);
        MessageResponse response = await result.MatchAsync(
            async state => {
                if (!state) {
                    logger.Warning("Failed to delete lorescope at id {id}", command.LoreScopeId);
                    return MessageResponse.FromErrorString("Failed to delete lorescope");
                }

                await messageBroker.InvokeLoreScopeDeletedAsync(command.LoreScopeId, Mode.WaitForNone, ct: ct);
                return MessageResponse.FromState(true);
            }, _ => {
                logger.Warning("Error occured during operation trying to delete lorescope at id {id}", command.LoreScopeId);
                return Task.FromResult(MessageResponse.FromErrorString("Failed to delete lorescope"));
            }
        );
        return response;
    }

    protected override ValueTask<bool> ValidateAccessAsync(DeleteLoreScopeRequest command, CancellationToken ct = default) 
        => protectionRules.CanAccessRepoWithPermission<ILoreScopeRepository>(
            command.LoreScopeId, 
            command.AccessingUser,
            PermissionsStore.LorescopeDelete,
            ct
        );
}
