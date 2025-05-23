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
public readonly partial struct PaginatedDataResult<T>() : IUnion<PaginatedData<T>, Error<string>> where T : notnull {
    public static PaginatedDataResult<T> FromError(string failure) => FromError(new Error<string>(failure));
}
