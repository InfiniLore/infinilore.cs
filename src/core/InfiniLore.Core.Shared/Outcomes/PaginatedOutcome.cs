// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Unions;
using InfiniLore.Core.Pagination;

namespace InfiniLore.Core.Outcomes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases(nameof(Success), "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct PaginatedOutcome<TSuccess>() : IUnion<PaginatedData<TSuccess>, ErrorOutcome> where TSuccess : class {
    public static implicit operator Task<PaginatedOutcome<TSuccess>>(PaginatedOutcome<TSuccess> outcome) => Task.FromResult(outcome);
    public static implicit operator ValueTask<PaginatedOutcome<TSuccess>>(PaginatedOutcome<TSuccess> outcome) => ValueTask.FromResult(outcome);
}

