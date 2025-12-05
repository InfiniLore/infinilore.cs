// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Core.Outcomes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly record struct AlreadyExists;
public readonly record struct Invalid;
public readonly record struct NotFound;
public readonly record struct ValidationFailed(IEnumerable<string>? Errors);