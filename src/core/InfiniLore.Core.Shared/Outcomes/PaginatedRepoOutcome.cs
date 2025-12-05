// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Unions;
using InfiniLore.Core.Pagination;

namespace InfiniLore.Core.Outcomes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("Data", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct PaginatedRepoOutcome<TSuccess>() : IUnion<PaginatedData<TSuccess>, RepoErrorOutcome> where TSuccess : class {
    public static implicit operator Task<PaginatedRepoOutcome<TSuccess>>(PaginatedRepoOutcome<TSuccess> outcome) => Task.FromResult(outcome);
    public static implicit operator ValueTask<PaginatedRepoOutcome<TSuccess>>(PaginatedRepoOutcome<TSuccess> outcome) => ValueTask.FromResult(outcome);
}