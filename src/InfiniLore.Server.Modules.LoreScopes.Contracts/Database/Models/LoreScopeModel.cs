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
    [MaxLength(Defaults.DescriptionMaxLength)] public string? Description { get; set; }
    
    public static class Defaults {
        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 1024;
    }
}
