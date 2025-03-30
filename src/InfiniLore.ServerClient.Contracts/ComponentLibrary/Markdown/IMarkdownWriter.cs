// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.ServerClient.ComponentLibrary.Markdown;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMarkdownWriter {
    IMarkdownWriter Write(string value);
    IMarkdownWriter Write(char value);
    IMarkdownWriter Write(int value);
    IMarkdownWriter Write(ReadOnlySpan<char> value);
    
    IMarkdownWriter WriteLine();
    IMarkdownWriter WriteLine(string value);
    IMarkdownWriter WriteFormatted(string format, params object[] args);
}
