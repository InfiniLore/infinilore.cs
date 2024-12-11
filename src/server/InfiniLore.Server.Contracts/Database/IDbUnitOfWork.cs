// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.Contracts.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IDbUnitOfWork<T> : IUnitOfWork where T : DbContext {
    /// <summary>
    ///     Asynchronously retrieves the current InfiniLoreDbContext instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the InfiniLoreDbContext instance.</returns>
    /// <remarks>Not using a getter property to ensure proper usage of CancellationToken</remarks>
    ValueTask<T> GetDbContextAsync(CancellationToken ct = default);
}
