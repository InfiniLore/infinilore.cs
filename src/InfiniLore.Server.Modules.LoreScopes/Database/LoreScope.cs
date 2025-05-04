// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database;
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Modules.LoreScopes.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScope : UserData {
    [MaxLength(Defaults.NameMaxLength)] public string Name { get; set; } = "";
    [MaxLength(Defaults.ShortDescriptionMaxLength)] public string ShortDescription { get; set; } = "";
    
    public static class Defaults {
        public const int NameMaxLength = 100;
        public const int ShortDescriptionMaxLength = 256;
    }
}
