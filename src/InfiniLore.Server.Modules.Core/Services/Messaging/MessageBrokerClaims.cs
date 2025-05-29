// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IMessageBroker>(IMessageBroker.Claims)]
public class MessageBrokerClaims(IAccessingUserProvider accessFactory) : IMessageBroker {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ValueTask<IAccessingUser> GetMessageAccessAsync(CancellationToken ct = default) 
        => ValueTask.FromResult(accessFactory.FromClaims(ct));
}
