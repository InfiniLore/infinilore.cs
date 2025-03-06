// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Server.Services.CQRS;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("State", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial struct MediatorResponse() : IUnion<bool, Error<string>> {

    public bool State => AsState;
    public bool TryGetState(out bool state) => TryGetAsState(out state);
    
    public static MediatorResponse FromFailureString(string value) => new() {
        IsError = true,
        AsError = new Error<string>(value)
    };
}

[UnionAliases("Success", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial struct MediatorResponse<T>() : IUnion<T, Error<string>> {
    
    public static MediatorResponse<T> FromFailureString(string value) => new() {
        IsError = true,
        AsError = new Error<string>(value)
    };
}