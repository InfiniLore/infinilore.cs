// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LoreScopes.Messaging.Commands;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PermissionsStore=InfiniLore.Modules.Core.Shared.PermissionsStore;

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
    protected override async Task<Outcome> HandleCommandAsync(DeleteLoreScopeRequest command, CancellationToken ct = default) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        Outcome result = await loreScopeRepo.DeleteByIdAsync(command.LoreScopeId, ct);
        Outcome outcome = await result.MatchAsync(
            async _ => {
                await messageBroker.InvokeLoreScopeDeletedAsync(command.LoreScopeId, Mode.WaitForNone, ct: ct);
                return Outcome.FromState(true);
            },
            _ => {
                logger.Warning("Failed to delete lorescope at id {id}", command.LoreScopeId);
                return Task.FromResult(Outcome.FromError("Failed to delete lorescope"));
            },
            _ => {
                logger.Warning("Error occured during operation trying to delete lorescope at id {id}", command.LoreScopeId);
                return Task.FromResult(Outcome.FromError("Failed to delete lorescope"));
            }
        );
        return outcome;
    }

    protected override ValueTask<bool> ValidateAccessAsync(DeleteLoreScopeRequest command, CancellationToken ct = default) 
        => protectionRules.CanAccessRepoWithPermission<ILoreScopeRepository>(
            command.LoreScopeId, 
            command.AccessingUser,
            PermissionsStore.LorescopeDelete,
            ct
        );
}
