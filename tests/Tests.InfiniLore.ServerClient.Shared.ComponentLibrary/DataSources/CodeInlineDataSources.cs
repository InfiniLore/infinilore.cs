// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class CodeInlineDataSources {
    private static readonly string SectionName = nameof(CodeInlineDataSources)[..^nameof(DataSources).Length];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<MarkdownTestDto>> DataSources() {
        
        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "This is an `example` of some inline code.",
            HtmlOutput: "<p>This is an <code>example</code> of some inline code.</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "`\\``",
            HtmlOutput: "<p><code>`</code></p>"
        );
    }
}
