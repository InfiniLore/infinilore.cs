// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace InfiniLore.Server.Database.Models.Data.System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class KeyValueStore {
    [MaxLength(Defaults.KeyMaxLength)] public required string Key { get; set; } = string.Empty;
    [MaxLength(Defaults.ValueMaxLength)] public string? Value { get; set; }

    public static class Defaults {
        public const int KeyMaxLength = 256;
        public const int ValueMaxLength = int.MaxValue - 1;
    }

    public bool TryGetConvertJsonValueToObject<TJsonObject>([NotNullWhen(true)] out TJsonObject? obj) where TJsonObject : class {
        obj = null;
        if (Value.IsNullOrWhiteSpace()) return false;
        
        // Try deserializing Value to the specified type TJsonObject
        try {
            obj = JsonSerializer.Deserialize<TJsonObject>(Value);
            return obj != null;
        } catch (JsonException) {
            return false;
        }
    }

    [MemberNotNullWhen(true, nameof(Value))]
    public bool TrySetbOjectAsJsonValue<TJsonObject>(TJsonObject obj) where TJsonObject : class {
        try {
            string json = JsonSerializer.Serialize(obj); 
            if (json.Length > Defaults.ValueMaxLength) { return false; }
            Value = json;
            return true;
        } catch (JsonException) {
            return false;
        }
    }
    
    public bool CanSetObjectAsValueJson<TJsonObject>(TJsonObject obj) where TJsonObject : class {
        try {
            string json = JsonSerializer.Serialize(obj); 
            return json.Length <= Defaults.ValueMaxLength;
        } catch (Exception ex) {
            Console.WriteLine(ex);
            return false;
        }
    }

}
