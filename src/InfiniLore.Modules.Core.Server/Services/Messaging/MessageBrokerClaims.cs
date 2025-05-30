// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Core.Server;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IMessageBroker>(IMessageBroker.FromClaims)]
public class MessageBrokerClaims(IAccessingUserProvider accessFactory) : IMessageBroker {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ValueTask<IAccessingUser> GetMessageAccessAsync(CancellationToken ct = default) 
        => ValueTask.FromResult(accessFactory.FromClaims(ct));
}
