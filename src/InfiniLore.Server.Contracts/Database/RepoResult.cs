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
public readonly partial struct RepoResult() : IUnion<Success, RepoResultFailure> {
    public static implicit operator bool(RepoResult value) => value.IsSuccess;
    
    public static readonly RepoResult Success = FromSuccess(new Success());

    public static RepoResult FromFailure(RepositoryFailures failure) => FromFailure(RepoResultFailure.FromKnownFailure(failure));
    public static RepoResult FromFailure(Failure<string> failure) => FromFailure(RepoResultFailure.FromUnknownFailure(failure));
    public static RepoResult FromFailure(string failure) => FromFailure(RepoResultFailure.FromUnknownFailure(new Failure<string>(failure)));
}

[UnionAliases("Success", "Failure")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial struct RepoResult<T>() : IUnion<T, RepoResultFailure> {
    public static implicit operator bool(RepoResult<T> value) => value.IsSuccess;
    
    public static RepoResult<T> FromFailure(RepositoryFailures failure) => FromFailure(RepoResultFailure.FromKnownFailure(failure));
    public static RepoResult<T> FromFailure(Failure<string> failure) => FromFailure(RepoResultFailure.FromUnknownFailure(failure));
    public static RepoResult<T> FromFailure(string failure) => FromFailure(RepoResultFailure.FromUnknownFailure(new Failure<string>(failure)));
}

[UnionAliases("KnownFailure", "UnknownFailure")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial struct RepoResultFailure() : IUnion<RepositoryFailures, Failure<string>>;