// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;
using InfiniLore.Shared;

namespace InfiniLore.Server.Modules.LsMarkdownFiles.Messaging.Queries;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetLsMarkdownFilesByOwnerQuery(
    Guid OwnerId,
    QueryConfig QueryConfig = default,
    Pagination Pagination = default
) : MessageRequest<PaginatedData<LsMarkdownFileModel>>;
