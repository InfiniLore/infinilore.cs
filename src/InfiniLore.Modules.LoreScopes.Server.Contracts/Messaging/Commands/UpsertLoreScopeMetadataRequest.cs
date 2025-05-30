// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.Messaging;

namespace InfiniLore.Server.Modules.LoreScopes.Messaging.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record UpsertLoreScopeMetadataRequest(
    Guid LoreScopeId,
    string? Name = null,
    string? Description = null
) : MessageRequest;
