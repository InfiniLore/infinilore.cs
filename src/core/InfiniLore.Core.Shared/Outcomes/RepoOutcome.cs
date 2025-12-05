// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Unions;

namespace InfiniLore.Core.Outcomes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("Data", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct RepoOutcome<TData>() : IUnion<TData, RepoErrorOutcome> {
    public static implicit operator Task<RepoOutcome<TData>>(RepoOutcome<TData> outcome) => Task.FromResult(outcome);
    public static implicit operator ValueTask<RepoOutcome<TData>>(RepoOutcome<TData> outcome) => ValueTask.FromResult(outcome);
}