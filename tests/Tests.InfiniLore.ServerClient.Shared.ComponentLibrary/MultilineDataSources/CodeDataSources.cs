// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.Encodings.Web;

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
                <code>
                    const code = sample();&#xD;&#xA;
                </code>
            </pre>
            """
        );
        yield return static () => new MultilineDataDto(
            Markdown: """
            ```javascript
            const code = sample();
            ```
            """,
            HtmlOutput: """
            <pre>
                <code class="language-javascript">
                    const code = sample();&#xD;&#xA;
                </code>
            </pre>
            """
        );
        
        yield return static () => {
            string htmlEncoded = HtmlEncoder.Default.Encode("tell application \"Foo\"\nbeep\nend tell");
            
            return new MultilineDataDto(
                Markdown: "```\ntell application \"Foo\"\nbeep\nend tell\n```",
                HtmlOutput: $"""
                <pre>
                    <code>
                        {htmlEncoded}&#xA;
                    </code>
                </pre>
                """
            );
        };
        
        yield return static () => {
            string htmlEncoded = HtmlEncoder.Default.Encode("""
                tell application "Foo"
                    beep
                end tell
                """);
            return new MultilineDataDto(
                Markdown: """
                ```
                tell application "Foo"
                    beep
                end tell
                ```
                """,
                HtmlOutput: $"""
                <pre>
                    <code>
                       {htmlEncoded}&#xD;&#xA;
                    </code>
                </pre>
                """
            );
        };
        
        yield return static () => new MultilineDataDto(
            Markdown: """
            ```
            **some valid markdown**
            ```
            """,
            HtmlOutput: """
            <pre><code>
                **some valid markdown**&#xD;&#xA;
            </code></pre>
            """
        );
    }
}
