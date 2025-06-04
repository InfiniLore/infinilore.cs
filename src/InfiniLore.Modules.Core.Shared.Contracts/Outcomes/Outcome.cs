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
        state = IsTrue;
        return IsTrue || IsFalse;
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
        Func<TOutput> trueCase,
        Func<TOutput> falseCase,
        Func<string, TOutput> errorCase
    ) => this switch {
        { IsTrue: true } => trueCase(),
        { IsFalse: true } => falseCase(),
        { IsAccessRefused: true, AsAccessRefused: var value} => errorCase(value.Reason),
        { IsError: true, AsError: var value } => errorCase(value.Value),
        _ => throw new ArgumentException("Union does not contain a value")
    };

    public async Task<TOutput> MatchAsync<TOutput>(
        Func<Task<TOutput>> trueCase,
        Func<Task<TOutput>> falseCase,
        Func<string, Task<TOutput>> errorCase
    ) => this switch {
        { IsTrue: true } => await trueCase(),
        { IsFalse: true } => await falseCase(),
        { IsAccessRefused: true, AsAccessRefused: var value} => await errorCase(value.Reason),
        { IsError: true, AsError: var value } => await errorCase(value.Value),
        _ => throw new ArgumentException("Union does not contain a value")
    };
    
    public TOutput Match<TOutput>(
        Func<bool, TOutput> boolCase,
        Func<string, TOutput> errorCase
    ) => this switch {
        { IsTrue: true } => boolCase(true),
        { IsFalse: true } => boolCase(false),
        { IsAccessRefused: true, AsAccessRefused: var value} => errorCase(value.Reason),
        { IsError: true, AsError: var value } => errorCase(value.Value),
        _ => throw new ArgumentException("Union does not contain a value")
    };

    public async Task<TOutput> MatchAsync<TOutput>(
        Func<bool, Task<TOutput>> boolCase,
        Func<string, Task<TOutput>> errorCase
    ) => this switch {
        { IsTrue: true } => await boolCase(true),
        { IsFalse: true } => await boolCase(false),
        { IsAccessRefused: true, AsAccessRefused: var value} => await errorCase(value.Reason),
        { IsError: true, AsError: var value } => await errorCase(value.Value),
        _ => throw new ArgumentException("Union does not contain a value")
    };

    public async Task<TOutput> MatchAsync<TOutput>(
        Func<bool, CancellationToken, Task<TOutput>> boolCase,
        Func<string, CancellationToken, Task<TOutput>> errorCase,
        CancellationToken ct
    ) => this switch {
        { IsTrue: true } => await boolCase(true, ct),
        { IsFalse: true } => await boolCase(false, ct),
        { IsAccessRefused: true, AsAccessRefused: var value} => await errorCase(value.Reason, ct),
        { IsError: true, AsError: var value } => await errorCase(value.Value, ct),
        _ => throw new ArgumentException("Union does not contain a value")
    };
    #endregion
    
    #region Switch Overloads
    public void Switch(
        Action trueCase,
        Action falseCase,
        Action<string> errorCase
    ){
        switch (this) {
            case {IsTrue: true} : trueCase(); return;
            case {IsFalse: true} : falseCase(); return;
            case {IsAccessRefused: true, AsAccessRefused: var value} : errorCase(value.Reason); return;
            case {IsError: true, AsError: var value} : errorCase(value.Value); return;
        }
        throw new ArgumentException("Union does not contain a value");
    }
    
    public void Switch(
        Action<bool> boolCase,
        Action<string> errorCase
    ){
        switch (this) {
            case {IsTrue: true, AsTrue: var value} : boolCase(value); return;
            case {IsFalse: true, AsFalse: var value} : boolCase(value); return;
            case {IsAccessRefused: true, AsAccessRefused: var value } : errorCase(value.Reason); return;
            case {IsError: true, AsError: var value} : errorCase(value.Value); return;
        }
        throw new ArgumentException("Union does not contain a value");
    }

    public async Task SwitchAsync(
        Func<bool, Task> boolCase,
        Func<string, Task> errorCase
    ){
        switch (this) {
            case {IsTrue: true, AsTrue: var value} : await boolCase(value); return;
            case {IsFalse: true, AsFalse: var value} : await boolCase(value); return;
            case {IsAccessRefused: true, AsAccessRefused: var value } : await errorCase(value.Reason); return;
            case {IsError: true, AsError: var value} : await errorCase(value.Value); return;
        }
        throw new ArgumentException("Union does not contain a value");
    }
    
    public async Task SwitchAsync(
        Func<bool, CancellationToken, Task> boolCase,
        Func<string, CancellationToken, Task> errorCase,
        CancellationToken ct
    ){
        switch (this) {
            case {IsTrue: true, AsTrue: var value} : await boolCase(value, ct); return;
            case {IsFalse: true, AsFalse: var value} : await boolCase(value, ct); return;
            case {IsAccessRefused: true, AsAccessRefused: var value } : await errorCase(value.Reason, ct); return;
            case {IsError: true, AsError: var value} : await errorCase(value.Value, ct); return;
        }
        throw new ArgumentException("Union does not contain a value");
    }
    #endregion
}

[UnionAliases("Data", "AccessRefused", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public partial record struct Outcome<T>() : IUnion<T, AccessRefused, Error<string>> {
    
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
        Func<string, TOutput> errorCase
    ) => this switch {
        { IsData: true, AsData: var value } => dataCase(value),
        { IsAccessRefused: true, AsAccessRefused: var value } => errorCase(value.Reason),
        { IsError: true, AsError: var value } => errorCase(value.Value),
        _ => throw new ArgumentException("Union does not contain a valid value")
    };

    public async Task<TOutput> MatchAsync<TOutput>(
        Func<T, Task<TOutput>> dataCase,
        Func<string, Task<TOutput>> errorCase
    ) => this switch {
        { IsData: true, AsData: var value } => await dataCase(value),
        { IsAccessRefused: true, AsAccessRefused: var value } => await errorCase(value.Reason),
        { IsError: true, AsError: var value } => await errorCase(value.Value),
        _ => throw new ArgumentException("Union does not contain a valid value")
    };

    public async Task<TOutput> MatchAsync<TOutput>(
        Func<T, CancellationToken, Task<TOutput>> dataCase,
        Func<string, CancellationToken, Task<TOutput>> errorCase,
        CancellationToken ct
    ) => this switch {
        { IsData: true, AsData: var value } => await dataCase(value, ct),
        { IsAccessRefused: true, AsAccessRefused: var value } => await errorCase(value.Reason, ct),
        { IsError: true, AsError: var value } => await errorCase(value.Value, ct),
        _ => throw new ArgumentException("Union does not contain a valid value")
    };
    #endregion
    
    #region Switch Overloads
    public void Switch(
        Action<T> dataCase,
        Action<string> errorCase
    ){
        switch (this) {
            case {IsData: true, AsData: var value} : dataCase(value); return;
            case {IsAccessRefused: true, AsAccessRefused: var value } : errorCase(value.Reason); return;
            case {IsError: true, AsError: var value} : errorCase(value.Value); return;
        }
        throw new ArgumentException("Union does not contain a value");
    }

    public async Task SwitchAsync(
        Func<T, Task> dataCase,
        Func<string, Task> errorCase
    ){
        switch (this) {
            case {IsData: true, AsData: var value} : await dataCase(value); return;
            case {IsAccessRefused: true, AsAccessRefused: var value } : await  errorCase(value.Reason); return;
            case {IsError: true, AsError: var value} : await errorCase(value.Value); return;
        }
        throw new ArgumentException("Union does not contain a value");
    }

    public async Task SwitchAsync(
        Func<T, CancellationToken, Task> dataCase,
        Func<string, CancellationToken, Task> errorCase,
        CancellationToken ct
    ){
        switch (this) {
            case {IsData: true, AsData: var value} : await dataCase(value, ct); return;
            case {IsAccessRefused: true, AsAccessRefused: var value } : await  errorCase(value.Reason, ct); return;
            case {IsError: true, AsError: var value} : await errorCase(value.Value, ct); return;
        }
        throw new ArgumentException("Union does not contain a value");
    }
    #endregion
}
