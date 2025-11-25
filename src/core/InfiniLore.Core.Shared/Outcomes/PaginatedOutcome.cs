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
    public static implicit operator PaginatedOutcome<TSuccess>(PaginatedOutcome outcome) => outcome.Match(
        successCase: _ => throw new InvalidOperationException("Cannot convert PaginatedOutcome to PaginatedOutcome<TSuccess> when Success case is present."),
        errorCase: FromError
    );
    
    public static implicit operator Task<PaginatedOutcome<TSuccess>>(PaginatedOutcome<TSuccess> outcome) => Task.FromResult(outcome);
    public static implicit operator ValueTask<PaginatedOutcome<TSuccess>>(PaginatedOutcome<TSuccess> outcome) => ValueTask.FromResult(outcome);
}

[UnionAliases(nameof(Success), "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct PaginatedOutcome() : IUnion<Success, ErrorOutcome> {
    public static implicit operator Task<PaginatedOutcome>(PaginatedOutcome outcome) => Task.FromResult(outcome);
    public static implicit operator ValueTask<PaginatedOutcome>(PaginatedOutcome outcome) => ValueTask.FromResult(outcome);
    
    public static PaginatedOutcome Failure { get; } = FromError(new Failure());
    public static PaginatedOutcome ValidationFailed { get; } = FromError(new ValidationFailed());
    
    
    public static PaginatedOutcome<TSuccess> FromSuccess<TSuccess>(PaginatedData<TSuccess> success) where TSuccess : class => PaginatedOutcome<TSuccess>.FromSuccess(success);
}

