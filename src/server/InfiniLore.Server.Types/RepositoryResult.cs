// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Server.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("Success", "Failure")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial struct RepoResult() : IUnion<Success, Failure<string>> {
    public static implicit operator RepoResult(string input) => new Failure<string>(input);
    public static implicit operator RepoResult(bool value) => value ? new Success() : new Failure<string>();

    public static implicit operator bool(RepoResult value) => value.IsSuccess;
}

[UnionAliases("Success", "Failure")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial struct RepoResult<T>() : IUnion<T, Failure<string>> {
    public static implicit operator RepoResult<T>(string input) => new Failure<string>(input);
    public static implicit operator bool(RepoResult<T> value) => value.IsSuccess;

    public SuccessOrFailure<T> ToSuccessOrFailure() {
        if (IsSuccess) return AsSuccess;
        return AsFailure;
    }
}

[UnionAliases("PaginatedResult", "Failure")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial struct PaginatedRepoResult<T>() : IUnion<PaginatedResult<T>, Failure<string>> {
    public static implicit operator PaginatedRepoResult<T>(string input) => new Failure<string>(input);
    public static implicit operator bool(PaginatedRepoResult<T> value) => value.IsPaginatedResult;

    public SuccessOrFailure<PaginatedResult<T>> ToSuccessOrFailure() {
        if (IsPaginatedResult) return new Success<PaginatedResult<T>>(AsPaginatedResult);
        return AsFailure;
    }
}