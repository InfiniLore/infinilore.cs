// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Database.Repositories.Content;
using InfiniLore.Server.Contracts.Database;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Database.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents an abstract base class for repositories that handle entities within an MSSQL database context.
/// </summary>
/// <typeparam name="T">The type of entity that the repository will manage.</typeparam>
public abstract class MsSqlRepository<T>(IUnitOfWork unitOfWork) : IRepository where T : class {
    #region Queryables
    /// <summary>
    ///     Asynchronously retrieves the current instance of the MsSqlDbContext.
    /// </summary>
    /// <param name="ct">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the MsSqlDbContext instance.</returns>
    public async ValueTask<MsSqlDbContext> GetDbContextAsync(CancellationToken ct = default) => await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);

    /// <summary>
    ///     Asynchronously retrieves the DbSet for the specified entity type <typeparamref name="T" />.
    ///     Should only be used by the default repositories <see cref="BaseContentRepository{T}" /> and
    ///     <see cref="UserContentRepository{T}" /> to premake some generally accessible methods.
    ///     All other direct repositories should use the actual DbSet contained in the DbContext.
    /// </summary>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete. Optional.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, with a result of the DbSet for the specified type
    ///     <typeparamref name="T" />.
    /// </returns>
    protected async ValueTask<DbSet<T>> GetDbSetAsync(CancellationToken ct = default) {
        MsSqlDbContext dbContext = await GetDbContextAsync(ct);
        return dbContext.Set<T>();
    }
    #endregion
}
