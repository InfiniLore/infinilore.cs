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
public readonly partial record struct PaginatedRepoOutcome<TSuccess>() : IUnion<PaginatedData<TSuccess>, RepoErrorOutcome> where TSuccess : class {
    public static implicit operator PaginatedRepoOutcome<TSuccess>(PaginatedRepoOutcome outcome) => outcome.Match(
        successCase: _ => throw new InvalidOperationException("Cannot convert PaginatedRepoOutcome to PaginatedRepoOutcome<TSuccess> when Success case is present."),
        errorCase: FromError
    );
    
    public static implicit operator Task<PaginatedRepoOutcome<TSuccess>>(PaginatedRepoOutcome<TSuccess> outcome) => Task.FromResult(outcome);
    public static implicit operator ValueTask<PaginatedRepoOutcome<TSuccess>>(PaginatedRepoOutcome<TSuccess> outcome) => ValueTask.FromResult(outcome);
}

[UnionAliases(nameof(Success), "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct PaginatedRepoOutcome() : IUnion<Success, RepoErrorOutcome>{
    public static PaginatedRepoOutcome AlreadyExists { get; } = FromError(new AlreadyExists());
    public static PaginatedRepoOutcome Invalid { get;} = FromError(new Invalid());
    public static PaginatedRepoOutcome NotFound { get;} = FromError(new NotFound());
}
