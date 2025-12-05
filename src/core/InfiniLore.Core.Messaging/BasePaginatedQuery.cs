// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Core.Database;
using InfiniLore.Core.Outcomes;
using InfiniLore.Core.Pagination;

namespace InfiniLore.Core.Messaging;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract record BasePaginatedQuery<TResult> : ICommand<PaginatedOutcome<TResult>> where TResult : class {
    public QueryConfig Config { get; init; } = QueryConfig.None;
    public PaginationData Pagination { get; init; } = PaginationData.Default;
}