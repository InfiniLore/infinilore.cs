// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text;

namespace InfiniLore.ServerClient.ComponentLibrary.Markdown;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMarkdownParser {
    string ParseMultiline(string markdown);
    void ParseMultiline(string markdown, StringBuilder builder);
    
    string ParseSingleline(string markdown);
    void ParseSingleline(string markdown, StringBuilder builder, SingleLineOrigin origin = SingleLineOrigin.Undefined);
}
