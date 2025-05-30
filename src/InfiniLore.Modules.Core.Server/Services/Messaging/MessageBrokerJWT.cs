// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Core.Server;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IMessageBroker>(IMessageBroker.FromJwtToken)]
public class MessageBrokerJwt(IAccessingUserProvider accessFactory) : IMessageBroker {
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ValueTask<IAccessingUser> GetMessageAccessAsync(CancellationToken ct = default)
        => accessFactory.FromJwtTokenAsync(ct);
}
