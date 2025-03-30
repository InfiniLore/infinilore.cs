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
            """
            ```
            const code = sample();
            ```
            """,
            """
            <pre>
                <code>
                    const code = sample();&#xD;&#xA;
                </code>
            </pre>
            """
        );
        yield return static () => new MarkdownTestDto(SectionName,
            """
            ```javascript
            const code = sample();
            ```
            """,
            """
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
                "```\ntell application \"Foo\"\nbeep\nend tell\n```",
                $"""
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
                """
                ```
                tell application "Foo"
                    beep
                end tell
                ```
                """,
                $"""
                <pre>
                    <code>
                       {htmlEncoded}&#xD;&#xA;
                    </code>
                </pre>
                """
            );
        };

        yield return static () => new MarkdownTestDto(SectionName,
            """
            ```
            **some valid markdown**
            ```
            """,
            """
            <pre><code>
                **some valid markdown**&#xD;&#xA;
            </code></pre>
            """
        );
    }
}
