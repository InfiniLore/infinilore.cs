// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace InfiniLore.Server.Modules.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IKeyValueEntry {
    string Key { get; }
    string? Value { get; set; }

    public bool TryGetConvertJsonValueToObject<TJsonObject>([NotNullWhen(true)] out TJsonObject? decodedObject) where TJsonObject : class;
    
    [MemberNotNullWhen(true, nameof(Value))]
    public bool TrySetObjectAsJsonValue<TJsonObject>(in TJsonObject objectToEncode) where TJsonObject : class;
}
