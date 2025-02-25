// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Database.Models.Data.System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class KeyValueStore : SystemData {
    [MaxLength(Defaults.KeyMaxLength)] public required string Key { get; set; } = string.Empty;
    [MaxLength(Defaults.ValueMaxLength)] public string? Value { get; set; }

    public static class Defaults {
        public const int KeyMaxLength = 256;
        public const int ValueMaxLength = 1024;
    }
}
