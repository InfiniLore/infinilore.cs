// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Modules.Core.Messaging.Commands;

namespace InfiniLore.Server.Modules.Core.Messaging;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class IMessageBrokerExtensions {
    public static async ValueTask<MessageResponse<Guid>> CreateInfiniLoreUser(this IMessageBroker factory, string auth0Id, string userName, CancellationToken ct = default) {
        // TODO this has to be a method or something that auto switches from claims to JWT depending on the context (keyed service?)
        IMessageAccess access = factory.AccessFactory.FromClaims(ct);
        
        var request = new UserCreateRequest(auth0Id, userName) {
            Access = access
        };

        return await request.ExecuteAsync(ct: ct);
    }
}
