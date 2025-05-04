// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("Success", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial struct PaginatedResult<T>() : IUnion<PaginatedData<T>, Error<string>> {
    public static implicit operator bool(PaginatedResult<T> value) => value.IsSuccess;

    public static PaginatedResult<T> FromError(string failure) => FromError(new Error<string>(failure));
}
