// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.ServerClient.ComponentLibrary.Markdown;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.MarkdownWriters;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TextWriterMarkdownWriter<T>(T writer) : IMarkdownWriter  where T : TextWriter {
    public IMarkdownWriter Write(string value) {
        writer.Write(value);
        return this;
    }
    
    public IMarkdownWriter Write(char value) {
        writer.Write(value);
        return this;
    }
    
    public IMarkdownWriter Write(int value) {
        writer.Write(value);
        return this;
    }
    
    public IMarkdownWriter Write(ReadOnlySpan<char> value) {
        writer.Write(value);
        return this;
    }

    public IMarkdownWriter WriteLine() {
        writer.WriteLine();
        return this;
    }

    public IMarkdownWriter WriteLine(string value) {
        writer.WriteLine(value);
        return this;
    }

    public IMarkdownWriter WriteFormatted(string format, params object[] args) {
        writer.Write(format, args);
        return this;
    }
}
