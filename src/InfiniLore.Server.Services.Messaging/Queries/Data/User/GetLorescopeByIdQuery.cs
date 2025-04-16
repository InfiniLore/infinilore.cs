// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models.Data.User;

namespace InfiniLore.Server.Services.Messaging.Queries.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetLorescopeByIdQuery(Guid LorescopeId, Guid UserId = default, bool AutoInclude = false) : MediatorRequest<LoreScope> {
    public bool IsLoreScopeOnly => UserId == Guid.Empty;
}
