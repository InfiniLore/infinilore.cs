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
    
    public TOutput Match<TOutput>(
        Func<PaginatedData<T>, TOutput> dataCase,
        Func<Error<string>, TOutput> errorCase
    ) => this switch {
        { IsData: true, AsData: var value } => dataCase(value),
        { IsAccessRefused: true } => throw new InvalidOperationException("AccessRefused is not design to be a valid response for this union."),
        { IsError: true, AsError: var value } => errorCase(value),
        _ => throw new ArgumentException("Union does not contain a valid value")
    };

    public async Task<TOutput> MatchAsync<TOutput>(
        Func<PaginatedData<T>, Task<TOutput>> dataCase,
        Func<Error<string>, Task<TOutput>> errorCase
    ) => this switch {
        { IsData: true, AsData: var value } => await dataCase(value),
        { IsAccessRefused: true } => throw new InvalidOperationException("AccessRefused is not design to be a valid response for this union."),
        { IsError: true, AsError: var value } => await errorCase(value),
        _ => throw new ArgumentException("Union does not contain a valid value")
    };
}
