// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using System.Diagnostics.CodeAnalysis;

namespace InfiniLore.Server.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("Success", "Failure")]
public readonly partial struct RepoResult() : IUnion<Success, Failure<string>> {
    public static implicit operator RepoResult(string input) => new Failure<string>(input);
    public static implicit operator RepoResult(bool value) => value ? new Success() : new Failure<string>();

    public static implicit operator bool(RepoResult value) => value.IsSuccess;
}

[UnionAliases("Success", "Failure")]
public readonly partial struct RepoResult<T>() : IUnion<Success<T>, Failure<string>> {
    public bool TryGetSuccessValue([NotNullWhen(true)] out T? value) {
        if (IsSuccess) {
            value = AsSuccess.Value;
            return value is not null;
        }

        value = default;
        return false;
    }

    public static implicit operator RepoResult<T>(string input) => new Failure<string>(input);
    public static implicit operator RepoResult<T>(T value) => new Success<T>(value);

    public static implicit operator bool(RepoResult<T> value) => value.IsSuccess;

    public SuccessOrFailure<T> ToSuccessOrFailure() {
        if (IsSuccess) return AsSuccess;

        return AsFailure;
    }
}

[UnionAliases("Success", "Failure")]
public readonly partial struct PaginatedRepoResult<T>() : IUnion<Success<PaginatedResult<T>>, Failure<string>> {
    public bool TryGetSuccessValue([NotNullWhen(true)] out PaginatedResult<T>? value) {
        if (IsSuccess) {
            value = AsSuccess.Value;
            return true;
        }
        
        value = null;
        return false;
    }

    public static implicit operator PaginatedRepoResult<T>(string input) => new Failure<string>(input);
    public static implicit operator PaginatedRepoResult<T>(PaginatedResult<T> value) => new Success<PaginatedResult<T>>(value);

    public static implicit operator bool(PaginatedRepoResult<T> value) => value.IsSuccess;

    public SuccessOrFailure<PaginatedResult<T>> ToSuccessOrFailure() {
        if (IsSuccess) return AsSuccess;
        return AsFailure;
    }
}