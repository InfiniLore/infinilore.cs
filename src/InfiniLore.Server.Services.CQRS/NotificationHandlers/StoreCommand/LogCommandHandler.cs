// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Services.Cqrs;
using InfiniLore.Server.Services.CQRS.Notifications.Data.System;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InfiniLore.Server.Services.CQRS.NotificationHandlers.StoreCommand;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LogCommandHandler(ILogger<LogCommandHandler> logger) : INotificationHandler<StoreCommandNotification> {
    public Task Handle(StoreCommandNotification notification, CancellationToken ct) {
        IMediatorRequest request = notification.Request;
        string json = JsonSerializer.Serialize(request);
        logger.Debug("Command received: {json}", json);
        
        return Task.CompletedTask;
    }
}
