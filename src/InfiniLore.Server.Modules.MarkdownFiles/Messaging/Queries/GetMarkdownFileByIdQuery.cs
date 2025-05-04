// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.MarkdownFiles.Database;

namespace InfiniLore.Server.Modules.MarkdownFiles.Messaging.Queries;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetMarkdownFileByIdQuery(Guid MarkdownFileId, Guid LorescopeId = default, bool AutoInclude = false) : MessageRequest<MarkdownFile>;
