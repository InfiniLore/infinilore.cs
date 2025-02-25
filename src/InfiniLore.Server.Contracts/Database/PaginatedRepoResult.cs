// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Server.Contracts.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("Success", "Failure")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial struct PaginatedRepoResult<T>() : IUnion<PaginatedResult<T>, RepoResultFailure> {
    public static implicit operator bool(PaginatedRepoResult<T> value) => value.IsSuccess;

    public static PaginatedRepoResult<T> FromFailure(RepositoryFailures failure) => FromFailure(RepoResultFailure.FromKnownFailure(failure));
    public static PaginatedRepoResult<T> FromFailure(Failure<string> failure) => FromFailure(RepoResultFailure.FromUnknownFailure(failure));
    public static PaginatedRepoResult<T> FromFailure(string failure) => FromFailure(RepoResultFailure.FromUnknownFailure(new Failure<string>(failure)));
}
