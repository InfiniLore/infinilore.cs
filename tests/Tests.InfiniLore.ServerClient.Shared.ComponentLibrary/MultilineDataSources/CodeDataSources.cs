// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.MultilineDataSources;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class CodeDataSources {
    public static IEnumerable<Func<MultilineDataDto>> Data() {
        
        yield return static () => new MultilineDataDto(
            Markdown: """
            ```
            const code = sample();
            ```
            """,
            HtmlOutput: """
            <pre>
                <code lang="">
                    const code = sample();&#xD;&#xA;
                </code>
            </pre>
            """
        );
    }
}
