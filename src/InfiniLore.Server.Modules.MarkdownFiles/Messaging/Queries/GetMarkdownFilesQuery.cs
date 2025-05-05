// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.MarkdownFiles.Database;
using InfiniLore.Shared;

namespace InfiniLore.Server.Modules.MarkdownFiles.Messaging.Queries;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetMarkdownFilesQuery(
    Guid LorescopeId,
    PaginationInfo PaginationInfo,
    bool AutoInclude = false,
    bool Reverse = false
) : MessageRequest<PaginatedData<MarkdownFileModel>>;
