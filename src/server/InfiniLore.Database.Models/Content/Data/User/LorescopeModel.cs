// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Database.Models.Content.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LorescopeModel : UserContent {
    [MaxLength(64)] public required string Name { get; set; }
    [MaxLength(512)] public string Description { get; set; } = string.Empty;
}
