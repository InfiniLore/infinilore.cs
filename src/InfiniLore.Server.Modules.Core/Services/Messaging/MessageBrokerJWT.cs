// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IMessageBroker>(IMessageBroker.JwtToken)]
public class MessageBrokerJwt(IMessageAccessFactory accessFactory) : IMessageBroker {
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ValueTask<IMessageAccess> GetMessageAccessAsync(CancellationToken ct = default)
        => accessFactory.FromJwtTokenAsync(ct);
}
