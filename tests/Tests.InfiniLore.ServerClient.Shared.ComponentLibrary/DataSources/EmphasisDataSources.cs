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
            "Example of **bold** and *italic* and ***bold italic***.",
            "<p>Example of <strong>bold</strong> and <em>italic</em> and <strong><em>bold italic</em></strong>.</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "**bold**",
            "<p><strong>bold</strong></p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "*italic*",
            "<p><em>italic</em></p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "***bold italic***",
            "<p><strong><em>bold italic</em></strong></p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "**bold *nested italic***",
            "<p><strong>bold <em>nested italic</em></strong></p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "**bold *nested \\* italic***",
            "<p><strong>bold <em>nested * italic</em></strong></p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "** \\* **",
            "<p><strong> * </strong></p>"
        );
        yield return static () => new MarkdownTestDto(SectionName,
            "* \\* *",
            "<p><em> * </em></p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "***nested italic* bold**",
            "<p><strong><em>nested italic</em> bold</strong></p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "**bold** *italic*",
            "<p><strong>bold</strong> <em>italic</em></p>"
        );
    }
}
