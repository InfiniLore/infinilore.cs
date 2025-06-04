// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("True","False", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public partial record struct RepoOutcome() : IUnion<True, False, Error<string>> {
    public static RepoOutcome True { get; } = FromTrue();
    public static RepoOutcome False { get; } = FromFalse();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetAsState(out bool state) {
        state = IsTrue;
        return IsTrue || IsFalse;
    }
    
    public Outcome ToOutcome()
        => Match(
            Outcome.FromState,
            Outcome.FromError
        );
    
    #region From Overloads
    public static PaginatedRepoOutcome<T> FromData<T>(PaginatedData<T> data) where T : class 
        => PaginatedRepoOutcome<T>.FromData(data);
    
    public static RepoOutcome<T> FromData<T>(T data) 
        => RepoOutcome<T>.FromData(data);
    
    public static RepoOutcome FromState(bool state)
        => state ? new True() : new False();
    
    public static RepoOutcome FromError(string value) 
        => FromError(new Error<string>(value));
    
    public static RepoOutcome<T> FromError<T>(string value) 
        => FromError(new Error<string>(value));

    public static RepoOutcome FromTrue()
        => FromTrue(new True());

    public static RepoOutcome FromFalse()
        => FromFalse(new False());
    #endregion

    #region Match Overloads
    
    public TOutput Match<TOutput>(
        Func<bool, TOutput> boolCase,
        Func<Error<string>, TOutput> errorCase
    ) => this switch {
        { IsTrue: true, AsTrue: var value } => boolCase(value),
        { IsFalse: true, AsFalse: var value } => boolCase(value),
        { IsError: true, AsError: var value } => errorCase(value),
        _ => throw new ArgumentException("Union does not contain a value")
    };

    public async Task<TOutput> MatchAsync<TOutput>(
        Func<bool, Task<TOutput>> boolCase,
        Func<Error<string>, Task<TOutput>> errorCase
    ) => this switch {
        { IsTrue: true, AsTrue: var value } => await boolCase(value),
        { IsFalse: true, AsFalse: var value } => await boolCase(value),
        { IsError: true, AsError: var value } => await errorCase(value),
        _ => throw new ArgumentException("Union does not contain a value")
    };
    #endregion
    
    #region Switch Overloads
    public void Switch(
        Action<bool> boolCase,
        Action<Error<string>> errorCase
    ){
        switch (this) {
            case {IsTrue: true, AsTrue: var value} : boolCase(value); return;
            case {IsFalse: true, AsFalse: var value} : boolCase(value); return;
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
            case {IsError: true, AsError: var value} : await errorCase(value); return;
        }
        throw new ArgumentException("Union does not contain a value");
    }
    #endregion
}

[UnionAliases("Data", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public partial record struct RepoOutcome<T>() : IUnion<T, Error<string>> {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static implicit operator RepoOutcome<T>(RepoOutcome outcomeWithError) {
        return outcomeWithError.Match(
            _ => throw new InvalidOperationException("Cannot convert a response with a boolean response to a response with data."),
            _ => throw new InvalidOperationException("Cannot convert a response with a boolean response to a response with data."),
            FromError
        );
    }
    
    public Outcome<T> ToOutcome()
        => Match(
            Outcome<T>.FromData,
            Outcome<T>.FromError
        );
    
    #region From Overloads
    public static RepoOutcome<T> FromError(string value)
        => FromError(new Error<string>(value));
    #endregion
}
