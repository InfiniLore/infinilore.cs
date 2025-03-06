// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Database.Models.Data.User;
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
