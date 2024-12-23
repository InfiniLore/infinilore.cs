// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models;
using InfiniLore.Server.Types;
using System.Linq.Expressions;

namespace InfiniLore.Server.Contracts.Database.Repositories.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Provides asynchronous methods to retrieve data based on specified criteria for a collection of objects of type
///     <typeparamref name="T" />.
/// </summary>
/// <typeparam name="T">The type of objects handled by these methods, which must derive from <see cref="BasicContent" />.</typeparam>
public interface IHasTryGetByCriteriaAsync<T> where T : BasicContent {
    /// Attempts to retrieve an array of entities from the repository that match the specified criteria.
    /// This method supports various overloads to include criteria, ordering, and pagination options.
    /// <param name="predicate">
    ///     An expression that defines the criteria for selecting entities. Entities that satisfy this expression will
    ///     be included in the result.
    /// </param>
    /// <param name="ct">
    ///     A CancellationToken to observe while waiting for the task to complete. It allows the operation to be canceled.
    ///     This parameter is optional and has a default value if not specified.
    /// </param>
    /// <return>
    ///     A ValueTask containing a RepoResult, which holds an array of entities of type T that match
    ///     the specified criteria or an error message if the operation fails.
    /// </return>
    ValueTask<RepoResult<T[]>> TryGetByCriteriaAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

    /// <summary>
    ///     Attempts to retrieve an array of entities of type <typeparamref name="T" /> that match the specified criteria.
    /// </summary>
    /// <typeparam name="T">The type of the entities to retrieve, constrained to <see cref="BasicContent" />.</typeparam>
    /// <param name="predicate">
    ///     An expression that represents the criteria for filtering the entities. This can be a function
    ///     accepting an index and entity to decide if it fulfills the conditions.
    /// </param>
    /// <param name="ct">
    ///     A cancellation token to observe while waiting for the task to complete. Defaults to
    ///     <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a <see cref="RepoResult{T}" />
    ///     structure
    ///     that describes the success or failure of the operation. If successful, it contains the filtered array of entities.
    /// </returns>
    ValueTask<RepoResult<T[]>> TryGetByCriteriaAsync(Expression<Func<T, int, bool>> predicate, CancellationToken ct = default);

    /// <summary>
    ///     Attempts to retrieve a collection of items that match the specified criteria asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of objects to be retrieved, constrained to <see cref="BasicContent" />.</typeparam>
    /// <param name="predicate">A predicate to filter the objects of type <typeparamref name="T" />.</param>
    /// <param name="orderBy">An expression to order the result set by a specific property.</param>
    /// <param name="pageInfo">The pagination information to apply to the query.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> containing a <see cref="RepoResult{T}" /> that holds
    ///     the result of the query operation, either a successful collection of items or an error message.
    /// </returns>
    ValueTask<RepoResult<T[]>> TryGetByCriteriaAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy, PaginationInfo pageInfo, CancellationToken ct = default);

    /// <summary>
    ///     Asynchronously attempts to retrieve an array of items from the repository that match the specified criteria.
    /// </summary>
    /// <typeparam name="T">The type of content being queried, which must derive from <see cref="BasicContent" />.</typeparam>
    /// <param name="predicate">An expression representing the criteria that each element must satisfy.</param>
    /// <param name="orderBy">
    ///     An optional expression used for ordering the resulting items, based on a property of the type
    ///     <typeparamref name="T" />.
    /// </param>
    /// <param name="pageInfo">
    ///     An object containing pagination information, specifying the page number and page size for the
    ///     query results.
    /// </param>
    /// <param name="ct">A cancellation token that may be used to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a <see cref="RepoResult{T}" />
    ///     which is an array of items of type <typeparamref name="T" /> that match the specified criteria.
    /// </returns>
    ValueTask<RepoResult<T[]>> TryGetByCriteriaAsync(Expression<Func<T, int, bool>> predicate, Expression<Func<T, object>> orderBy, PaginationInfo pageInfo, CancellationToken ct = default);
}
