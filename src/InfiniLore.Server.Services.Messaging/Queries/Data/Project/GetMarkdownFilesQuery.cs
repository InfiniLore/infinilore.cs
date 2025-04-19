// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Database.Models.Data.Project;

namespace InfiniLore.Server.Services.Messaging.Queries.Data.Project;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetMarkdownFilesQuery(
    Guid LorescopeId,
    PaginationInfo PaginationInfo = default,
    bool AutoInclude = false,
    bool Reverse = false
) : MessageRequest<PaginatedData<MarkdownFile>>;
