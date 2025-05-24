// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("Data", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue | UnionExtra.GenerateFrom)]
public readonly partial struct PaginatedResult<T>() : IUnion<PaginatedData<T>, Error<string>> where T : class {
    public static PaginatedResult<T> FromError(string failure) => FromError(new Error<string>(failure));
}
