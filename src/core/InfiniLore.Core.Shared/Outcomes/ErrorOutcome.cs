// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Unions;

namespace InfiniLore.Core.Outcomes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct ErrorOutcome() : IUnion<Failure, ValidationFailed> {
    
    public static ErrorOutcome ValidationFailed { get; } = FromValidationFailed(new ValidationFailed());
    public static ErrorOutcome Failure { get; } = FromFailure(new Failure());
}
