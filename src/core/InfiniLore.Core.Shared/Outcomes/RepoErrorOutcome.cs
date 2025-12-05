// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Unions;

namespace InfiniLore.Core.Outcomes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases(nameof(AlreadyExists), nameof(Invalid), nameof(NotFound))]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial record struct RepoErrorOutcome() : IUnion<AlreadyExists, Invalid, NotFound> {
    public static RepoErrorOutcome AlreadyExists { get; } = FromAlreadyExists(new AlreadyExists());
    public static RepoErrorOutcome Invalid { get; } = FromInvalid(new Invalid());
    public static RepoErrorOutcome NotFound { get; } = FromNotFound(new NotFound());
}

