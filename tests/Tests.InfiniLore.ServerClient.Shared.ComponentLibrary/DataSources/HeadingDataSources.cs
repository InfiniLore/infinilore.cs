// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class HeadingDataSources {
    private static readonly string SectionName = nameof(HeadingDataSources)[..^nameof(DataSources).Length];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<MarkdownTestDto>> DataSources() {
        for (int i = 1; i < 7; i++) {
            string heading = new('#', i);
            int depth = i;
            yield return () => new MarkdownTestDto(SectionName,
                Markdown: $"{heading} Heading",
                HtmlOutput: $"<h{depth}>Heading</h{depth}>"
            );
        }

        yield return () => new MarkdownTestDto(SectionName,
            Markdown: """
            Heading
            ---
            """,
            HtmlOutput: "<h1>Heading</h1>"
        );
        
        yield return () => new MarkdownTestDto(SectionName,
            Markdown: """
            Heading
            ===
            """,
            HtmlOutput: "<h1>Heading</h1>"
        );
        
        yield return () => new MarkdownTestDto(SectionName,
            Markdown: """
            Heading
                ========
            """,
            HtmlOutput: "<h1>Heading</h1>"
        );
    }
}
