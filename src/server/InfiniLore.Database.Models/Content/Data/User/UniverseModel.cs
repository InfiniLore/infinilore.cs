// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Database.Models.Content.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UniverseModel : UserContent {
    public required MultiverseModel Multiverse { get; set; }
    public Guid MultiverseId { get; set; }

    [MaxLength(64)] public required string Name { get; set; }
    [MaxLength(512)] public required string Description { get; set; } = string.Empty;
}
