// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class RandomDataSources {
    private static readonly string SectionName = nameof(RandomDataSources)[..^nameof(DataSources).Length];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<MarkdownTestDto>> DataSources() {
        
        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: """
            ## Try CommonMark
            
            You can try CommonMark here.  This dingus is powered by [commonmark.js](https://github.com/commonmark/commonmark.js), the JavaScript reference implementation.
            
            1. item one
            2. item two
               - sublist
               - sublist
            """,
            HtmlOutput: """
            <h2>Try CommonMark</h2>
            <p>You can try CommonMark here.  This dingus is powered by
            <a href="https://github.com/commonmark/commonmark.js">commonmark.js</a>, the
            JavaScript reference implementation.</p>
            <ol>
                <li>item one</li>
                <li>item two
                <ul>
                    <li>sublist</li>
                    <li>sublist</li>
                </ul>
                </li>
            </ol>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: """
            ## Try CommonMark

            You can try CommonMark here.  This dingus is powered 
            by [commonmark.js](https://github.com/commonmark/commonmark.js),
            the JavaScript reference implementation.

            1. item one
            2. item two
               - sublist
               - sublist
            """,
            HtmlOutput: """
            <h2>Try CommonMark</h2>
            <p>You can try CommonMark here.  This dingus is powered by
            <a href="https://github.com/commonmark/commonmark.js">commonmark.js</a>, the
            JavaScript reference implementation.</p>
            <ol>
                <li>item one</li>
                <li>item two
                <ul>
                    <li>sublist</li>
                    <li>sublist</li>
                </ul>
                </li>
            </ol>
            """
        );
    }
}
