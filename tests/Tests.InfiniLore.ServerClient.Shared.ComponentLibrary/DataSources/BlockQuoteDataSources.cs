// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class BlockQuoteDataSources {
    private static readonly string SectionName = nameof(BlockQuoteDataSources)[..^nameof(DataSources).Length];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<MarkdownTestDto>> DataSources() {
        
        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: """
            > # test
            > > test
            >
            > - test
            > - test
            """,
            HtmlOutput: """
            <blockquote>
                <h1>test</h1>
                <blockquote>
                    <p>test</p>
                </blockquote>
                <ul>
                    <li>test</li>
                    <li>test</li>
                </ul>
            </blockquote>
            """
        );yield return static () => new MarkdownTestDto(SectionName,
            Markdown: """
            > blockQuote 1
            >> ...blockQuote 2
            >>> ...blockQuote 3
            """,
            HtmlOutput: """
            <blockquote>
                <p>blockQuote 1</p>
                <blockquote>
                    <p>...blockQuote 2</p>
                    <blockquote>
                        <p>...blockQuote 3</p>
                    </blockquote>
                </blockquote>
            </blockquote>
            """
        );
    }
}
