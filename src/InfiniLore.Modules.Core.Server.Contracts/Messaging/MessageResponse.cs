// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using JetBrains.Annotations;

namespace InfiniLore.Modules.Core.Server.Messaging;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("State", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public partial record MessageResponse : IUnion<bool, Error<ICollection<string>>> {

    public bool State => AsState;
    public bool TryGetState(out bool? state) => TryGetAsState(out state);

    public static MessageResponse FromErrorString(string value) => new() {
        IsError = true,
        AsError = new Error<ICollection<string>>([value])
    };

    public static MessageResponse FromErrorString(ICollection<string> value) => new() {
        IsError = true,
        AsError = new Error<ICollection<string>>(value)
    };

    public static implicit operator MessageResponse(string value) => FromErrorString(value);
    public static implicit operator MessageResponse(Error<string> error) => FromErrorString(error.Value);

    public static MessageResponse<T> FromSuccess<T>(T data) => MessageResponse<T>.FromSuccess(data);
}

[UnionAliases("Success", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public partial record MessageResponse<T>() : IUnion<T, Error<ICollection<string>>> {

    // Used by ValidateRequestBehaviour
    [UsedImplicitly] public MessageResponse(string error) : this() {
        FromErrorString(error);
    }

    public static MessageResponse<T> FromErrorString(string value) => new() {
        IsError = true,
        AsError = new Error<ICollection<string>>([value])
    };

    public static MessageResponse<T> FromErrorString(ICollection<string> value) => new() {
        IsError = true,
        AsError = new Error<ICollection<string>>(value)
    };

    public static implicit operator MessageResponse<T>(string value) => FromErrorString(value);
    public static implicit operator MessageResponse<T>(Error<string> error) => FromErrorString(error.Value);
    public static implicit operator MessageResponse<T>(MessageResponse responseWithError) {
        if (!responseWithError.TryGetAsError(out Error<ICollection<string>>? value)) throw new InvalidOperationException();
        return value;    
    }
}
