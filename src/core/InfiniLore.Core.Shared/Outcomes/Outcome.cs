// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Unions;

namespace InfiniLore.Core.Outcomes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------


[UnionAliases(nameof(Success), "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct Outcome<TSuccess>() : IUnion<TSuccess, ErrorOutcome> {
    public static implicit operator Outcome<TSuccess>(Outcome outcome) => outcome.Match(
        successCase: _ => throw new InvalidOperationException("Cannot convert Outcome to Outcome<TSuccess> when Success case is present."),
        errorCase: FromError
    );
    
    public static implicit operator Task<Outcome<TSuccess>>(Outcome<TSuccess> outcome) => Task.FromResult(outcome);
    public static implicit operator ValueTask<Outcome<TSuccess>>(Outcome<TSuccess> outcome) => ValueTask.FromResult(outcome);
}

[UnionAliases(nameof(Success), "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct Outcome() : IUnion<Success, ErrorOutcome> {
    public static implicit operator Task<Outcome>(Outcome outcome) => Task.FromResult(outcome);
    public static implicit operator ValueTask<Outcome>(Outcome outcome) => ValueTask.FromResult(outcome);
    
    public static Outcome Success { get;} = FromSuccess(new Success());
    
    public static Outcome Failure { get; } = FromError(new Failure());
    public static Outcome ValidationFailed { get; } = FromError(new ValidationFailed());
    
    
    public static Outcome<TSuccess> FromSuccess<TSuccess>(TSuccess success) => Outcome<TSuccess>.FromSuccess(success);
}

