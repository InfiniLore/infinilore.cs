// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Database.Models.Data.Project;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownFile : ProjectData {
    [MaxLength(Defaults.NameMaxLength)] public string Name { get; set; } = string.Empty;
    [MaxLength(Defaults.SourceMaxLength)] public string Source { get; set; } = string.Empty;
    
    public static class Defaults {
        public const int NameMaxLength = 64;
        public const int SourceMaxLength = int.MaxValue;
    }
}
