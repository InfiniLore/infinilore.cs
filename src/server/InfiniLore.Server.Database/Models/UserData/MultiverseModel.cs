// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.Database.Models.UserData;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MultiverseModel : UserContent {
    public required LoreScopeModel LoreScope { get; set; }
    public Guid LoreScopeId { get; set; }
    
    [MaxLength(64)] public required string Name { get; set; }
    [MaxLength(512)] public required string Description { get; set; } = string.Empty;
    
    public ICollection<UniverseModel> Universes { get; init; } = [];
}
