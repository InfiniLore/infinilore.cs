// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.MultilineDataSources;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class HeadingDataSources {
    public static IEnumerable<Func<MultilineDataDto>> Data() {
        for (int i = 1; i < 7; i++) {
            string heading = new('#', i);
            int depth = i;
            yield return () => new MultilineDataDto(
                Markdown: $"{heading} Heading",
                HtmlOutput: $"<h{depth}>Heading</h{depth}>"
            );
        }

        yield return () => new MultilineDataDto(
            Markdown: """
            Heading
            ---
            """,
            HtmlOutput: "<h1>Heading</h1>"
        );
        
        yield return () => new MultilineDataDto(
            Markdown: """
            Heading
            ===
            """,
            HtmlOutput: "<h1>Heading</h1>"
        );
        
        yield return () => new MultilineDataDto(
            Markdown: """
            Heading
                ========
            """,
            HtmlOutput: "<h1>Heading</h1>"
        );
    }
}
