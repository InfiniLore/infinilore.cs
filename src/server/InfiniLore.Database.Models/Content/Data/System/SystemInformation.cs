// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Database.Models.Content.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SystemInformation {
    [Key, MaxLength(512)] public required string Name { get; init; } // Only value can be changed after it is defined
    [MaxLength(512)] public required string Value { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    private SystemInformation() {}
    
    public SystemInformation FromString(string name, string value) => new() { Name = name, Value = value };
    public SystemInformation FromGuid(string name, Guid guid) => new() { Name = name, Value = guid.ToString() };
    public SystemInformation FromInt(string name, int intValue) => new() { Name = name, Value = intValue.ToString() };
    public SystemInformation FromBool(string name, bool boolValue) => new() { Name = name, Value = boolValue.ToString() };
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetAsGuid(out Guid guid) => Guid.TryParse(Value, out guid);
    public bool TryGetAsInt(out int intValue) => int.TryParse(Value, out intValue);
    public bool TryGetAsBool(out bool boolValue) => bool.TryParse(Value, out boolValue);
}
