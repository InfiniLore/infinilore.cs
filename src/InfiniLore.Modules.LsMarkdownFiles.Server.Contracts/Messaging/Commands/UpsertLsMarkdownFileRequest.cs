// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.Messaging;

namespace InfiniLore.Server.Modules.LsMarkdownFiles.Messaging.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record UpsertLsMarkdownFileRequest(
    Guid LoreScopeId,
    string FileName,
    Stream FileStream,
    Guid KnownLsMarkdownFileId = default
) : MessageRequest;
