// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IMessageBroker>(IMessageBroker.Server)]
public class MessageBrokerServer(IAccessingUserProvider accessFactory) : IMessageBroker {
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ValueTask<IAccessingUser> GetMessageAccessAsync(CancellationToken ct = default)
        => ValueTask.FromResult(accessFactory.Server);
}
