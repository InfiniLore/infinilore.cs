// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Server.Services.CQRS;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("Success", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial struct MediatorResponse<T>() : IUnion<T, Error<string>> {
    
    public static MediatorResponse<T> FromFailureString(string value) => new() {
        IsError = true,
        AsError = new Error<string>(value)
    };
}