// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Old.InfiniLore.Database.Models.Content.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SystemInformation {
    [Key, MaxLength(256)] public required string Name { get; init; } // Only value can be changed after it is defined
    [MaxLength(1024)] public required string Value { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    private SystemInformation() {}
    
    public static SystemInformation FromString(string name, string value) => new() { Name = name, Value = value };
    public static SystemInformation FromGuid(string name, Guid guid) => new() { Name = name, Value = guid.ToString() };
    public static SystemInformation FromInt(string name, int intValue) => new() { Name = name, Value = intValue.ToString() };
    public static SystemInformation FromBool(string name, bool boolValue) => new() { Name = name, Value = boolValue.ToString() };
    public static SystemInformation FromDateTime(string name, DateTime dateTime) => new() { Name = name, Value = dateTime.ToString(CultureInfo.InvariantCulture)};
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetAsGuid(out Guid guid) => Guid.TryParse(Value, out guid);
    public bool TryGetAsInt(out int intValue) => int.TryParse(Value, out intValue);
    public bool TryGetAsBool(out bool boolValue) => bool.TryParse(Value, out boolValue);
    public bool TryGetAsDateTime(out DateTime dateTime) => DateTime.TryParse(Value, out dateTime);
}
