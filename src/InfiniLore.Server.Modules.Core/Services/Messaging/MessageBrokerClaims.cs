// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IMessageBroker>(IMessageBroker.Claims)]
public class MessageBrokerClaims(IMessageAccessFactory accessFactory) : IMessageBroker {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ValueTask<IMessageAccess> GetMessageAccessAsync(CancellationToken ct = default) 
        => ValueTask.FromResult(accessFactory.FromClaims(ct));
}
