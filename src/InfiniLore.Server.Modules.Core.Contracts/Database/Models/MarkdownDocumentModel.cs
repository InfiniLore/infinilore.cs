// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace InfiniLore.Server.Modules.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownDocumentModel<TOwner> : OwnedModel<TOwner>
    where TOwner : BasicModel 
{
    [MaxLength(Defaults.TitleMaxLength)] public string? Title { get; set; }
    
    private string? _content;
    public string Content {
        get => _content ?? string.Empty;
        set {
            if (_content == value) return;
            _content = value;
            ContentHash = ComputeHash(_content);
        }
    }
    public string? ContentHash { get; set; }

    private string? _cachedRenderedHtml;
    public string CachedRenderedHtml {
        get => _cachedRenderedHtml ?? string.Empty;
        set {
            if (_cachedRenderedHtml == value) return;
            _cachedRenderedHtml = value;
            CachedRenderedHtmlHash = ComputeHash(_cachedRenderedHtml);
        }
    }
    public string? CachedRenderedHtmlHash { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Extra Data
    // -----------------------------------------------------------------------------------------------------------------
    public static class Defaults {
        public const int TitleMaxLength = 256;
    }

    private static string ComputeHash(string input) {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = SHA256.HashData(bytes);
        return Convert.ToHexString(hashBytes);
    }
}
