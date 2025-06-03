// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Modules.Core.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("True","False", "AccessRefused", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public partial record struct Outcome() : IUnion<True, False, AccessRefused, Error<string>> {
    public bool TryGetAsState(out bool state) {
        if (!IsTrue || !IsFalse) {
            state = false;
            return false;
        }
        state = IsTrue || !IsFalse;
        return true;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static Outcome FromError(string value) 
        => FromError(new Error<string>(value));
    
    public static Outcome FromAccessRefused(string value) 
        => FromAccessRefused(new AccessRefused(value));
    
    public TOutput Match<TOutput>(
        Func<True, TOutput> trueCase,
        Func<False, TOutput> falseCase,
        Func<Error<string>, TOutput> errorCase
    ) => this switch {
        { IsTrue: true, AsTrue: var value } => trueCase(value),
        { IsFalse: true, AsFalse: var value } => falseCase(value),
        { IsAccessRefused: true } => throw new InvalidOperationException("AccessRefused is not design to be a valid response for this union."),
        { IsError: true, AsError: var value } => errorCase(value),
        _ => throw new ArgumentException("Union does not contain a value")
    };

    public async Task<TOutput> MatchAsync<TOutput>(
        Func<True, Task<TOutput>> trueCase,
        Func<False, Task<TOutput>> falseCase,
        Func<Error<string>, Task<TOutput>> errorCase
    ) => this switch {
        { IsTrue: true, AsTrue: var value } => await trueCase(value),
        { IsFalse: true, AsFalse: var value } => await falseCase(value),
        { IsAccessRefused: true } => throw new InvalidOperationException("AccessRefused is not design to be a valid response for this union."),
        { IsError: true, AsError: var value } => await errorCase(value),
        _ => throw new ArgumentException("Union does not contain a value")
    };
}

[UnionAliases("Data", "AccessRefused", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public partial record struct Outcome<T>() : IUnion<T, AccessRefused, Error<string>> {
    
    public static implicit operator Outcome<T>(Outcome outcomeWithError) {
        return outcomeWithError.Match(
            _ => throw new InvalidOperationException("Cannot convert a response with a boolean response to a response with data."),
            _ => throw new InvalidOperationException("Cannot convert a response with a boolean response to a response with data."),
            FromAccessRefused,
            FromError
        );
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static Outcome<T> FromError(string value)
        => FromError(new Error<string>(value));
    
    public static Outcome<T> FromAccessRefused(string value)
        => FromAccessRefused(new AccessRefused(value));
    
    public TOutput Match<TOutput>(
        Func<T, TOutput> dataCase,
        Func<Error<string>, TOutput> errorCase
    ) => this switch {
        { IsData: true, AsData: var value } => dataCase(value),
        { IsAccessRefused: true } => throw new InvalidOperationException("AccessRefused is not design to be a valid response for this union."),
        { IsError: true, AsError: var value } => errorCase(value),
        _ => throw new ArgumentException("Union does not contain a valid value")
    };

    public async Task<TOutput> MatchAsync<TOutput>(
        Func<T, Task<TOutput>> dataCase,
        Func<Error<string>, Task<TOutput>> errorCase
    ) => this switch {
        { IsData: true, AsData: var value } => await dataCase(value),
        { IsAccessRefused: true } => throw new InvalidOperationException("AccessRefused is not design to be a valid response for this union."),
        { IsError: true, AsError: var value } => await errorCase(value),
        _ => throw new ArgumentException("Union does not contain a valid value")
    };
}
