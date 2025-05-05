// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.LoreScopes.Database;
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Modules.MarkdownFiles.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownFileModel : OwnedModel<LoreScopeModel> {
    [MaxLength(Defaults.NameMaxLength)] public string Name { get; set; } = string.Empty;
    [MaxLength(Defaults.SourceMaxLength)] public string Source { get; set; } = string.Empty;

    public static class Defaults {
        public const int NameMaxLength = 64;
        public const int SourceMaxLength = int.MaxValue;
    }
}
