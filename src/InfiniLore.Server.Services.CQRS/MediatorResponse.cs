// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using JetBrains.Annotations;

namespace InfiniLore.Server.Services.CQRS;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("State", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial struct MediatorResponse() : IUnion<bool, Error<ICollection<string>>> {

    public bool State => AsState;
    public bool TryGetState(out bool state) => TryGetAsState(out state);

    public static MediatorResponse FromErrorString(string value) => new() {
        IsError = true,
        AsError = new Error<ICollection<string>>([value])
    };

    public static MediatorResponse FromErrorString(ICollection<string> value) => new() {
        IsError = true,
        AsError = new Error<ICollection<string>>(value)
    };

    public static implicit operator MediatorResponse(string value) => FromErrorString(value);
    public static implicit operator MediatorResponse(Error<string> error) => FromErrorString(error.Value);
}

[UnionAliases("Success", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial struct MediatorResponse<T>() : IUnion<T, Error<ICollection<string>>> {

    // Used by ValidateRequestBehaviour
    [UsedImplicitly] public MediatorResponse(string error) : this() {
        FromErrorString(error);
    }

    public static MediatorResponse<T> FromErrorString(string value) => new() {
        IsError = true,
        AsError = new Error<ICollection<string>>([value])
    };

    public static MediatorResponse<T> FromErrorString(ICollection<string> value) => new() {
        IsError = true,
        AsError = new Error<ICollection<string>>(value)
    };

    public static implicit operator MediatorResponse<T>(string value) => FromErrorString(value);
    public static implicit operator MediatorResponse<T>(Error<string> error) => FromErrorString(error.Value);
}
