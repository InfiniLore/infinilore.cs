// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class KeyValueEntry : IKeyValueEntry {
    [MaxLength(Defaults.KeyMaxLength)] public required string Key { get; init; } = string.Empty;
    [MaxLength(Defaults.ValueMaxLength)] public string? Value { get; set; }

    public bool TryGetConvertJsonValueToObject<TJsonObject>([NotNullWhen(true)] out TJsonObject? decodedObject) where TJsonObject : class {
        decodedObject = null;
        if (Value.IsNullOrWhiteSpace()) return false;

        // Try deserializing Value to the specified type TJsonObject
        try {
            decodedObject = JsonSerializer.Deserialize<TJsonObject>(Value);
            return decodedObject != null;
        }
        catch (JsonException) {
            return false;
        }
    }

    [MemberNotNullWhen(true, nameof(Value))]
    public bool TrySetObjectAsJsonValue<TJsonObject>(in TJsonObject objectToEncode) where TJsonObject : class {
        try {
            string json = JsonSerializer.Serialize(objectToEncode);
            if (json.Length > Defaults.ValueMaxLength) return false;

            Value = json;
            return true;
        }
        catch (JsonException) {
            return false;
        }
    }

    public static bool CanSetObjectAsValueJson<TJsonObject>(in TJsonObject objectToEncode) where TJsonObject : class {
        try {
            string json = JsonSerializer.Serialize(objectToEncode);
            return json.Length <= Defaults.ValueMaxLength;
        }
        catch (Exception) {
            return false;
        }
    }

    public static class Defaults {
        public const int KeyMaxLength = 256;
        public const int ValueMaxLength = int.MaxValue - 1;
    }
}
