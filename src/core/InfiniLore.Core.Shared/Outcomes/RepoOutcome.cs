// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Core.Outcomes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases(nameof(Success), nameof(None))]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct RepoOutcome<TSuccess>() : IUnion<TSuccess, None> {
    public static implicit operator RepoOutcome<TSuccess>(RepoOutcome outcome) => outcome.Match(
        noneCase: FromNone
    );
    
    public static implicit operator Task<RepoOutcome<TSuccess>>(RepoOutcome<TSuccess> outcome) => Task.FromResult(outcome);
    public static implicit operator ValueTask<RepoOutcome<TSuccess>>(RepoOutcome<TSuccess> outcome) => ValueTask.FromResult(outcome);
}

[UnionAliases(nameof(None))]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct RepoOutcome() : IUnion<None>{
    public static RepoOutcome None { get; } = FromNone(new None());
}

