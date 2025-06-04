// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetUsersQuery(
    QueryConfig QueryConfig,
    Pagination Pagination
) : PaginatedMessageRequest<InfiniLoreUserModel>;
