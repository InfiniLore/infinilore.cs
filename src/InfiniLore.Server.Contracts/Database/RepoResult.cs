// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Server.Contracts.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("State", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial struct RepoResult() : IUnion<bool, Error<string>> {
    public bool State => AsState;
    public bool TryGetState(out bool state) => TryGetAsState(out state);
    
    public static implicit operator bool(RepoResult value) => value.IsState;

    public static RepoResult FromError(string failure) => FromError(new Error<string>(failure));
}

[UnionAliases("Success", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial struct RepoResult<T>() : IUnion<T, Error<string>> {
    public static implicit operator bool(RepoResult<T> value) => value.IsSuccess;
    public static RepoResult<T> FromError(string failure) => FromError(new Error<string>(failure));
};
