// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Unions;

namespace InfiniLore.Core.Outcomes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("Data", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct Outcome<TSuccess>() : IUnion<TSuccess, ErrorOutcome> {
    public static Outcome<TSuccess> Failure { get; } = FromError(new Failure());

    // -----------------------------------------------------------------------------------------------------------------
    // implicit operators
    // -----------------------------------------------------------------------------------------------------------------
    public static implicit operator Task<Outcome<TSuccess>>(Outcome<TSuccess> outcome) => Task.FromResult(outcome);
    public static implicit operator ValueTask<Outcome<TSuccess>>(Outcome<TSuccess> outcome) => ValueTask.FromResult(outcome);
}
