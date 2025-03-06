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
public readonly partial struct PaginatedRepoResult<T>() : IUnion<PaginatedResult<T>, Error<string>> {
    public static implicit operator bool(PaginatedRepoResult<T> value) => value.IsSuccess;

    public static PaginatedRepoResult<T> FromFailure(string failure) => FromFailure(new Error<string>(failure));
}
