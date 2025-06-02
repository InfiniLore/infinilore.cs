// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Core.Server.Messaging.Handlers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class EventReceiver<TEvent>(ILogger<EventReceiver<TEvent>> logger) : IEventHandler<TEvent> 
    where TEvent : IEvent
{
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task HandleAsync(TEvent eventModel, CancellationToken ct) {
        try {
            await ExecuteAsync(eventModel, ct);
        }
        catch (Exception e) {
            logger.Error(e, "Error while handling event {EventName} in {EventHandler}", typeof(TEvent).Name, GetType().Name);
        }
    }

    protected abstract Task ExecuteAsync(TEvent eventModel, CancellationToken ct);
}