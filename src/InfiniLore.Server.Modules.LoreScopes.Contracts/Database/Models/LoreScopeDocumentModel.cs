// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database;
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Modules.LoreScopes.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScopeDocumentModel : OwnedModel<LoreScopeModel> {
    [MaxLength(Defaults.ContentMaxLength)] public string Content { get; set; } = string.Empty;
    public string? HtmlRenderedContent { get; set; }
    
    public static class Defaults {
        public const int ContentMaxLength = 10_000;
    }
}
