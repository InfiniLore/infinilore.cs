// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Unions;

namespace InfiniLore.Core.Outcomes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases(nameof(AlreadyExists), nameof(Invalid))]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct RepoErrorOutcome() : IUnion<AlreadyExists, Invalid> {
    public static implicit operator Task<RepoErrorOutcome>(RepoErrorOutcome outcome) => Task.FromResult(outcome);
    public static implicit operator ValueTask<RepoErrorOutcome>(RepoErrorOutcome outcome) => ValueTask.FromResult(outcome);
}

