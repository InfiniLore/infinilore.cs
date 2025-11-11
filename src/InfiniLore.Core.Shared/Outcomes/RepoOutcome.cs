// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Core.Outcomes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases(nameof(Success), "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct RepoOutcome() : IUnion<Success, ErrorOutcome> {
    
    public static implicit operator Task<RepoOutcome>(RepoOutcome outcome) => Task.FromResult(outcome);
    public static implicit operator ValueTask<RepoOutcome>(RepoOutcome outcome) => ValueTask.FromResult(outcome);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetAsState(out bool state) {
        state = IsSuccess;
        return IsSuccess || IsError;
    }
}

[UnionAliases(nameof(Success), "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct RepoOutcome<TSuccess>() : IUnion<TSuccess, ErrorOutcome> {
    
    public static implicit operator Task<RepoOutcome<TSuccess>>(RepoOutcome<TSuccess> outcome) => Task.FromResult(outcome);
    public static implicit operator ValueTask<RepoOutcome<TSuccess>>(RepoOutcome<TSuccess> outcome) => ValueTask.FromResult(outcome);
}

[UnionAliases(nameof(Success), nameof(Failure), "UndefinedError")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct RepoOutcome<TSuccess, TFailure>() : IUnion<TSuccess, TFailure, ErrorOutcome> {
    
    public static implicit operator Task<RepoOutcome<TSuccess, TFailure>>(RepoOutcome<TSuccess, TFailure> outcome) => Task.FromResult(outcome);
    public static implicit operator ValueTask<RepoOutcome<TSuccess, TFailure>>(RepoOutcome<TSuccess, TFailure> outcome) => ValueTask.FromResult(outcome);
}

