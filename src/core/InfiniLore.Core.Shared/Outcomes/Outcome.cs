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
public readonly partial record struct Outcome() : IUnion<Success, ErrorOutcome> {
    
    public static implicit operator Task<Outcome>(Outcome outcome) => Task.FromResult(outcome);
    public static implicit operator ValueTask<Outcome>(Outcome outcome) => ValueTask.FromResult(outcome);

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
public readonly partial record struct Outcome<TSuccess>() : IUnion<TSuccess, ErrorOutcome> {
    
    public static implicit operator Task<Outcome<TSuccess>>(Outcome<TSuccess> outcome) => Task.FromResult(outcome);
    public static implicit operator ValueTask<Outcome<TSuccess>>(Outcome<TSuccess> outcome) => ValueTask.FromResult(outcome);
}

[UnionAliases(nameof(Success), nameof(Failure), "UndefinedError")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct Outcome<TSuccess, TFailure>() : IUnion<TSuccess, TFailure, ErrorOutcome> {
    
    public static implicit operator Task<Outcome<TSuccess, TFailure>>(Outcome<TSuccess, TFailure> outcome) => Task.FromResult(outcome);
    public static implicit operator ValueTask<Outcome<TSuccess, TFailure>>(Outcome<TSuccess, TFailure> outcome) => ValueTask.FromResult(outcome);
}

