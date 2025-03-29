// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.Encodings.Web;

namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class CodeDataSources {
    private static readonly string SectionName = nameof(CodeDataSources)[..^nameof(DataSources).Length];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<MarkdownTestDto>> DataSources() {
        
        yield return static () => new MarkdownTestDto(SectionName,
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
        yield return static () => new MarkdownTestDto(SectionName,
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
            
            return new MarkdownTestDto(SectionName,
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
            return new MarkdownTestDto(SectionName,
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
        
        yield return static () => new MarkdownTestDto(SectionName,
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
