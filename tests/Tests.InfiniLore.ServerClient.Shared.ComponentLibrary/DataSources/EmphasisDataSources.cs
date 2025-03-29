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
    }
}
