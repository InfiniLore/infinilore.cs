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
    public static Outcome True { get; } = FromTrue();
    public static Outcome False { get; } = FromFalse();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetAsState(out bool state) {
        if (!IsTrue || !IsFalse) {
            state = false;
            return false;
        }
        state = IsTrue || !IsFalse;
        return true;
    }
    
    #region From Overloads
    public static PaginatedOutcome<T> FromData<T>(PaginatedData<T> data) where T : class 
        => PaginatedOutcome<T>.FromData(data);
    
    public static Outcome<T> FromData<T>(T data) 
        => Outcome<T>.FromData(data);
    
    public static Outcome FromState(bool state)
        => state ? new True() : new False();
    
    public static Outcome FromError(string value) 
        => FromError(new Error<string>(value));

    public static Outcome FromTrue()
        => FromTrue(new True());

    public static Outcome FromFalse()
        => FromFalse(new False());
    
    public static Outcome FromAccessRefused(string value) 
        => FromAccessRefused(new AccessRefused(value));
    #endregion

    #region Match Overloads
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
    
    public TOutput Match<TOutput>(
        Func<bool, TOutput> boolCase,
        Func<Error<string>, TOutput> errorCase
    ) => this switch {
        { IsTrue: true, AsTrue: var value } => boolCase(value),
        { IsFalse: true, AsFalse: var value } => boolCase(value),
        { IsAccessRefused: true } => throw new InvalidOperationException("AccessRefused is not design to be a valid response for this union."),
        { IsError: true, AsError: var value } => errorCase(value),
        _ => throw new ArgumentException("Union does not contain a value")
    };

    public async Task<TOutput> MatchAsync<TOutput>(
        Func<bool, Task<TOutput>> boolCase,
        Func<Error<string>, Task<TOutput>> errorCase
    ) => this switch {
        { IsTrue: true, AsTrue: var value } => await boolCase(value),
        { IsFalse: true, AsFalse: var value } => await boolCase(value),
        { IsAccessRefused: true } => throw new InvalidOperationException("AccessRefused is not design to be a valid response for this union."),
        { IsError: true, AsError: var value } => await errorCase(value),
        _ => throw new ArgumentException("Union does not contain a value")
    };
    #endregion
    
    #region Switch Overloads
    public void Switch(
        Action<bool> boolCase,
        Action<AccessRefused> accessRefusedCase,
        Action<Error<string>> errorCase
    ){
        switch (this) {
            case {IsTrue: true, AsTrue: var value} : boolCase(value); return;
            case {IsFalse: true, AsFalse: var value} : boolCase(value); return;
            case {IsAccessRefused: true, AsAccessRefused: var value} : accessRefusedCase(value); return;
            case {IsError: true, AsError: var value} : errorCase(value); return;
        }
        throw new ArgumentException("Union does not contain a value");
    }

    public async Task SwitchAsync(
        Func<bool, Task> boolCase,
        Func<AccessRefused, Task> accessRefusedCase,
        Func<Error<string>, Task> errorCase
    ){
        switch (this) {
            case {IsTrue: true, AsTrue: var value} : await boolCase(value); return;
            case {IsFalse: true, AsFalse: var value} : await boolCase(value); return;
            case {IsAccessRefused: true, AsAccessRefused: var value} : await accessRefusedCase(value); return;
            case {IsError: true, AsError: var value} : await errorCase(value); return;
        }
        throw new ArgumentException("Union does not contain a value");
    }
    
    public void Switch(
        Action<bool> boolCase,
        Action<Error<string>> errorCase
    ){
        switch (this) {
            case {IsTrue: true, AsTrue: var value} : boolCase(value); return;
            case {IsFalse: true, AsFalse: var value} : boolCase(value); return;
            case {IsAccessRefused: true } : throw new InvalidOperationException("AccessRefused is not design to be a valid response for this union.");
            case {IsError: true, AsError: var value} : errorCase(value); return;
        }
        throw new ArgumentException("Union does not contain a value");
    }

    public async Task SwitchAsync(
        Func<bool, Task> boolCase,
        Func<Error<string>, Task> errorCase
    ){
        switch (this) {
            case {IsTrue: true, AsTrue: var value} : await boolCase(value); return;
            case {IsFalse: true, AsFalse: var value} : await boolCase(value); return;
            case {IsAccessRefused: true } : throw new InvalidOperationException("AccessRefused is not design to be a valid response for this union.");
            case {IsError: true, AsError: var value} : await errorCase(value); return;
        }
        throw new ArgumentException("Union does not contain a value");
    }
    #endregion
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
    #region From Overloads
    public static Outcome<T> FromError(string value)
        => FromError(new Error<string>(value));
    
    public static Outcome<T> FromAccessRefused(string value)
        => FromAccessRefused(new AccessRefused(value));
    #endregion

    #region Match Overloads
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
    #endregion
    
    #region Switch Overloads
    public void Switch(
        Action<T> dataCase,
        Action<Error<string>> errorCase
    ){
        switch (this) {
            case {IsData: true, AsData: var value} : dataCase(value); return;
            case {IsAccessRefused: true } : throw new InvalidOperationException("AccessRefused is not design to be a valid response for this union.");
            case {IsError: true, AsError: var value} : errorCase(value); return;
        }
        throw new ArgumentException("Union does not contain a value");
    }

    public async Task SwitchAsync(
        Func<T, Task> dataCase,
        Func<Error<string>, Task> errorCase
    ){
        switch (this) {
            case {IsData: true, AsData: var value} : await dataCase(value); return;
            case {IsAccessRefused: true } : throw new InvalidOperationException("AccessRefused is not design to be a valid response for this union.");
            case {IsError: true, AsError: var value} : await errorCase(value); return;
        }
        throw new ArgumentException("Union does not contain a value");
    }
    #endregion
}
