// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Data.Models.UserData;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UniverseModel : UserContent<UniverseModel> {
    public required MultiverseModel Multiverse { get; set; }
    public Guid MultiverseId { get; set; }

    [MaxLength(64)] public required string Name { get; set; }
    [MaxLength(512)] public required string Description { get; set; } = string.Empty;
}
