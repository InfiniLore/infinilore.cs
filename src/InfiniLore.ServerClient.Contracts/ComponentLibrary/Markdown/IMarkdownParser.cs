// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.ServerClient.ComponentLibrary.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMarkdownParser {
    string Parse(string markdown);
    void Parse<T>(string markdown, T writer) where T : TextWriter;

    void ParseMultiline(string markdown, IMarkdownWriter writer);
    void ParseSingleline(string markdown, IMarkdownWriter writer, SingleLineOrigin origin = SingleLineOrigin.Undefined);
}
