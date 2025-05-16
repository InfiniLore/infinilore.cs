// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Modules.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownDocumentModel : BasicModel {
    [MaxLength(Defaults.TitleMaxLength)] public string? Title { get; set; }
    public string? Content { get; set; }
    public string? ContentHash { get; set; }
    
    public string? CachedRenderedHtml { get; set; }
    public string? CachedRenderedHtmlHash { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Extra Data
    // -----------------------------------------------------------------------------------------------------------------
    public static class Defaults {
        public const int TitleMaxLength = 256;
    }
}
