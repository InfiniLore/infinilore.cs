// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Shared;

namespace InfiniLore.Server.Modules.LoreScopes.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetLoreScopesQuery(
    Guid UserId,
    bool AutoInclude = false,
    PaginationInfo PaginationInfo = default,
    bool Reverse = false
) : MessageRequest<PaginatedData<LoreScopeModel>>;
