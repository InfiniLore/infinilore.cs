// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Messaging;

namespace InfiniLore.Server.Modules.LoreScopes.Messaging.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record CreateLoreScopeRequest(
    Guid OwnerId,
    string LoreScopeName,
    string? LoreScopeDescription = null
) : MessageRequest<Guid>;
