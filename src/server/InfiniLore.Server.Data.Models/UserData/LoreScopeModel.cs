// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Data.Models.UserData;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScopeModel : UserContent<LoreScopeModel> {
    [MaxLength(64)] public required string Name { get; set; }
    [MaxLength(512)] public string Description { get; set; } = string.Empty;

    public ICollection<MultiverseModel> Multiverses { get; init; } = [];
}
