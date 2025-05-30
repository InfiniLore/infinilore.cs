// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Server.Modules.LoreScopes.Database;

namespace InfiniLore.Server.Modules.LoreScopes.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetLorescopeByIdQuery(Guid LorescopeId, Guid UserId = default, bool AutoInclude = false) : MessageRequest<LoreScopeModel> {
    public bool IsLoreScopeOnly => UserId == Guid.Empty;
}
