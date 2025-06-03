// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;

namespace InfiniLore.Server.Modules.LsMarkdownFiles.Messaging.Queries;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetLsMarkdownFilesByOwnerQuery(
    Guid OwnerId,
    QueryConfig QueryConfig = default,
    Pagination Pagination = default
) : MessageRequest<PaginatedData<LsMarkdownFileModel>>;
