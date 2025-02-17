// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Database.Models.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class KeyValueStore : SystemData{
    public const int MaxKeyLength = 255;
    [MaxLength(MaxKeyLength)] public required string Key { get; set; } = string.Empty;
    
    public const int MaxValueLength = 1024;
    [MaxLength(MaxValueLength)] public string? Value { get; set; }
}
