// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Shared;

namespace InfiniLore.Modules.Core.Server.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetUsersQuery(
    QueryConfig QueryConfig,
    PaginationInfo PaginationInfo
) : MessageRequest<PaginatedData<InfiniLoreUserModel>>;
