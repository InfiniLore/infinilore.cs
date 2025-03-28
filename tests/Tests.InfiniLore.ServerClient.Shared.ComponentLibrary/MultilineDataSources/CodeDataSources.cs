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
                <code lang="">
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
                    <code lang="">
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
                    <code lang="">
                       {htmlEncoded}&#xD;&#xA;
                    </code>
                </pre>
                """
            );
        };
    }
}
