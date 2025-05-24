// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Messaging;

namespace InfiniLore.Server.Modules.LoreScopes.Messaging.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record UpsertLoreScopeMetadataRequest(
    Guid LoreScopeId,
    string? Name = null,
    string? Description = null
) : MessageRequest;
