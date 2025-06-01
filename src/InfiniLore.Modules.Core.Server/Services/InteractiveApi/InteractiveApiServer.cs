// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Core.Server.InteractiveApi;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInteractiveApiServer>]
internal class InteractiveApiServer(
    [FromKeyedServices(IMessageBroker.FromClaims)] IMessageBroker messageBroker
) : IInteractiveApiServer {
    public IMessageBroker MessageBroker => messageBroker;
}