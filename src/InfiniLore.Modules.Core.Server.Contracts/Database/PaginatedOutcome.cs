// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("Data", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public partial record struct PaginatedRepoOutcome<T>() : IUnion<PaginatedData<T>, Error<string>> where T : class {
    public static implicit operator PaginatedRepoOutcome<T>(Outcome outcomeWithError) {
        return outcomeWithError.Match(
            _ => throw new InvalidOperationException("Cannot convert a response with a boolean response to a response with data."),
            _ => throw new InvalidOperationException("Cannot convert a response with a boolean response to a response with data."),
            FromError
        );
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static PaginatedRepoOutcome<T> FromError(string value) 
        => FromError(new Error<string>(value));

    public PaginatedOutcome<T> ToOutcome()
        => Match(
            PaginatedOutcome<T>.FromData, 
            PaginatedOutcome<T>.FromError
        );
}
