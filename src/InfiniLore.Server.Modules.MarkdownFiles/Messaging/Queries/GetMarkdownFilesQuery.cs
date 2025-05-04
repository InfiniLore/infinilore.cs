// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Contracts;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.MarkdownFiles.Database;

namespace InfiniLore.Server.Modules.MarkdownFiles.Messaging.Queries;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetMarkdownFilesQuery(
    Guid LorescopeId,
    PaginationInfo PaginationInfo,
    bool AutoInclude = false,
    bool Reverse = false
) : MessageRequest<PaginatedData<MarkdownFile>>;
