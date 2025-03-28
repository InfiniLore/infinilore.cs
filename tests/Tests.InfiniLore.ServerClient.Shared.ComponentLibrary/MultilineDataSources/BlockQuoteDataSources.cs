// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.MultilineDataSources;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class BlockQuoteDataSources {
    public static IEnumerable<Func<MultilineDataDto>> Data() {
        
        yield return static () => new MultilineDataDto(
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
        );yield return static () => new MultilineDataDto(
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
