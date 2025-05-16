// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.Users.Database;
using InfiniLore.Shared.Modules.LoreScopes.Database;
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Modules.LoreScopes.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScopeModel : OwnedModel<InfiniLoreUserModel>, ILoreScopeModel {
    [MaxLength(Defaults.NameMaxLength)] public string Name { get; set; } = "";
    [MaxLength(Defaults.ShortDescriptionMaxLength)] public string ShortDescription { get; set; } = "";
    
    public LoreScopeDocumentModel? Document { get; set; }
    public Guid DocumentId { get; set; } = Guid.Empty;
    
    
    public bool HasDocument => DocumentId != Guid.Empty;
    
    public static class Defaults {
        public const int NameMaxLength = 100;
        public const int ShortDescriptionMaxLength = 256;
    }
}
