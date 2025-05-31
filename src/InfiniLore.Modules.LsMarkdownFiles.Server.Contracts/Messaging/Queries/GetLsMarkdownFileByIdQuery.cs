// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;

namespace InfiniLore.Server.Modules.LsMarkdownFiles.Messaging.Queries;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetLsMarkdownFileByIdQuery(
    Guid FileId,
    QueryConfig QueryConfig = default
) : MessageRequest<LsMarkdownFileModel>;