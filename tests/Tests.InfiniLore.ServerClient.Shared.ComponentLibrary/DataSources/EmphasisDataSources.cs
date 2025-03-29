// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class EmphasisDataSources {
    private static readonly string SectionName = nameof(EmphasisDataSources)[..^nameof(DataSources).Length];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<MarkdownTestDto>> DataSources() {
        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "Example of **bold** and *italic* and ***bold italic***.",
            HtmlOutput: "<p>Example of <strong>bold</strong> and <em>italic</em> and <strong><em>bold italic</em></strong>.</p>"
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "**bold**",
            HtmlOutput: "<p><strong>bold</strong></p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "*italic*",
            HtmlOutput: "<p><em>italic</em></p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "***bold italic***",
            HtmlOutput: "<p><strong><em>bold italic</em></strong></p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "**bold *nested italic***",
            HtmlOutput: "<p><strong>bold <em>nested italic</em></strong></p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "***nested italic* bold**",
            HtmlOutput: "<p><strong><em>nested italic</em> bold</strong></p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "**bold** *italic*",
            HtmlOutput: "<p><strong>bold</strong> <em>italic</em></p>"
        );
    }
}
