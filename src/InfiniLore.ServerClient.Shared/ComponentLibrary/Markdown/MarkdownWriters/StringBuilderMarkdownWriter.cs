// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using System.Text;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.MarkdownWriters;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class StringBuilderMarkdownWriter(StringBuilder builder) : IMarkdownWriter {
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public StringBuilderMarkdownWriter() : this(new StringBuilder()) {}
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IMarkdownWriter Write(string value) {
        builder.Append(value);
        return this;
    }

    public IMarkdownWriter Write(char value) {
        builder.Append(value);
        return this;
    }

    public IMarkdownWriter Write(int value) {
        builder.Append(value);
        return this;
    }

    public IMarkdownWriter Write(ReadOnlySpan<char> value) {
        builder.Append(value);
        return this;
    }

    public IMarkdownWriter WriteLine() {
        builder.AppendLine();
        return this;
    }

    public IMarkdownWriter WriteLine(string value) {
        builder.AppendLine(value);
        return this;
    }

    public IMarkdownWriter WriteFormatted(string format, params object[] args) {
        builder.AppendFormat(format, args);
        return this;
    }

    public override string ToString() => builder.ToString();

    public void Clear() {
        builder.Clear();
    }
}
