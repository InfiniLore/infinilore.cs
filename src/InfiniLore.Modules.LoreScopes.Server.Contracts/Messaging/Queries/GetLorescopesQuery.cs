// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Shared;

namespace InfiniLore.Server.Modules.LoreScopes.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetLoreScopesByOwnerQuery(
    Guid UserId,
    QueryConfig QueryConfig = default,
    Pagination Pagination = default
) : MessageRequest<PaginatedData<LoreScopeModel>>;
