// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Shared;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("Success", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial struct PaginatedResult<T>() : IUnion<PaginatedData<T>, Error<string>> where T : class {
    public static implicit operator bool(PaginatedResult<T> value) => value.IsSuccess;

    public static PaginatedResult<T> FromError(string failure) => FromError(new Error<string>(failure));
}
