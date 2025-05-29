// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Messaging;

namespace InfiniLore.Server.Modules.LoreScopes.Messaging.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record UpsertLoreScopeImageRequest(
    Guid LoreScopeId,
    string FileName,
    string ContentType,
    Stream FileStream
) : MessageRequest;
