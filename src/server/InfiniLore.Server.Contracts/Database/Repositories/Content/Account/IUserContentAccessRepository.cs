// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models;

namespace InfiniLore.Server.Contracts.Database.Repositories.Content.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IUserContentAccessRepository {
    public ValueTask<bool> UserHasKindAsync(Guid contentId, Guid accessorId, AccessKind accessKind, CancellationToken ct = default);
}
