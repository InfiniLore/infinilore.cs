// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models;
using InfiniLore.Server.Contracts.Types;

namespace InfiniLore.Server.Contracts.Database.Repositories.Content.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IUserContentAccessRepository {
    public ValueTask<bool> UserHasKindAsync(Guid contentId, UserIdUnion accessorId, AccessKind accessKind, CancellationToken ct = default);
}
