// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Messaging;

namespace InfiniLore.Server.Modules.MarkdownFiles.Messaging.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record MarkdownFileAddOrUpdateRequest(
    Guid MarkdownFileId,
    Guid LoreScopeId,
    string FileName,
    string Source
) : MessageRequest<Guid>;
