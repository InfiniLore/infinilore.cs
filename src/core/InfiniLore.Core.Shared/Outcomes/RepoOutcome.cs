// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Unions;

namespace InfiniLore.Core.Outcomes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases(nameof(Success), nameof(None), "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct RepoOutcome<TSuccess>() : IUnion<TSuccess, None, RepoErrorOutcome> {
    public static implicit operator RepoOutcome<TSuccess>(RepoOutcome outcome) => outcome.Match(
        successCase: _ => throw new InvalidOperationException("Cannot convert RepoOutcome to RepoOutcome<TSuccess> when Success case is present."),
        noneCase: FromNone,
        errorCase: FromError
    );
    
    public static implicit operator Task<RepoOutcome<TSuccess>>(RepoOutcome<TSuccess> outcome) => Task.FromResult(outcome);
    public static implicit operator ValueTask<RepoOutcome<TSuccess>>(RepoOutcome<TSuccess> outcome) => ValueTask.FromResult(outcome);
}

[UnionAliases(nameof(Success), nameof(None), "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct RepoOutcome() : IUnion<Success, None, RepoErrorOutcome>{
    public static RepoOutcome Success { get;} = FromSuccess(new Success());
    public static RepoOutcome None { get; } = FromNone(new None());
    
    public static RepoOutcome AlreadyExists { get; } = FromError(new AlreadyExists());
    public static RepoOutcome Invalid { get;} = FromError(new Invalid());
}
