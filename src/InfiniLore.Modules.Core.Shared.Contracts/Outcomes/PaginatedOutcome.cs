// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Modules.Core.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("Data", "AccessRefused", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public partial record struct PaginatedOutcome<T>() : IUnion<PaginatedData<T>, AccessRefused, Error<string>> where T : class {
    public static implicit operator PaginatedOutcome<T>(Outcome outcomeWithError) {
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
    public static PaginatedOutcome<T> FromError(string value) 
        => FromError(new Error<string>(value));
    
    public static PaginatedOutcome<T> FromAccessRefused(string value)
        => FromAccessRefused(new AccessRefused(value));
    
    #region Match Overloads
    public TOutput Match<TOutput>(
        Func<PaginatedData<T>, TOutput> dataCase,
        Func<string, TOutput> errorCase
    ) => this switch {
        { IsData: true, AsData: var value } => dataCase(value),
        {IsAccessRefused: true, AsAccessRefused: var value} => errorCase(value.Reason),
        { IsError: true, AsError: var value } => errorCase(value.Value),
        _ => throw new ArgumentException("Union does not contain a valid value")
    };

    public Task<TOutput> MatchAsync<TOutput>(
        Func<PaginatedData<T>, Task<TOutput>> dataCase,
        Func<string, Task<TOutput>> errorCase
    ) => this switch {
        { IsData: true, AsData: var value } => dataCase(value),
        {IsAccessRefused: true, AsAccessRefused: var value} => errorCase(value.Reason),
        { IsError: true, AsError: var value } => errorCase(value.Value),
        _ => throw new ArgumentException("Union does not contain a valid value")
    };
    #endregion
    
    #region Switch Overloads
    public void Switch(
        Action<PaginatedData<T>> dataCase,
        Action<string> errorCase
    ){
        switch (this) {
            case {IsData: true, AsData: var value} : dataCase(value); return;
            case {IsAccessRefused: true, AsAccessRefused: var value} : errorCase(value.Reason); return;
            case {IsError: true, AsError: var value} : errorCase(value.Value); return;
        }
        throw new ArgumentException("Union does not contain a value");
    }

    public Task SwitchAsync(
        Func<PaginatedData<T>, Task> dataCase,
        Func<string, Task> errorCase
    ) {
        return this switch {
            { IsData: true, AsData: var value } => dataCase(value),
            { IsAccessRefused: true, AsAccessRefused: var value } => errorCase(value.Reason),
            { IsError: true, AsError: var value } => errorCase(value.Value),
            _ => throw new ArgumentException("Union does not contain a value")
        };
    }

    public Task SwitchAsync(
        Func<PaginatedData<T>, CancellationToken, Task> dataCase,
        Func<string, CancellationToken, Task> errorCase,
        CancellationToken ct
    ) {
        return this switch {
            { IsData: true, AsData: var value } => dataCase(value, ct),
            { IsAccessRefused: true, AsAccessRefused: var value } => errorCase(value.Reason, ct),
            { IsError: true, AsError: var value } => errorCase(value.Value, ct),
            _ => throw new ArgumentException("Union does not contain a value")
        };
    }
    #endregion
}
