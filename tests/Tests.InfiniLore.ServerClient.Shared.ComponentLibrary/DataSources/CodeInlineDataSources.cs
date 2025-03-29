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
        
        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "Here is some `inline code` inside a sentence.",
            HtmlOutput: "<p>Here is some <code>inline code</code> inside a sentence.</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "`code` at the start of the line.",
            HtmlOutput: "<p><code>code</code> at the start of the line.</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "This is the `last example`.",
            HtmlOutput: "<p>This is the <code>last example</code>.</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "Multiple `inline` `code` segments.",
            HtmlOutput: "<p>Multiple <code>inline</code> <code>code</code> segments.</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "Backticks inside inline code: ``Code with `backticks` inside``.",
            HtmlOutput: "<p>Backticks inside inline code: <code>Code with `backticks` inside</code>.</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "`Nested ` is not valid syntax.`",
            HtmlOutput: "<p><code>Nested </code> is not valid syntax.`</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "`inline code with special characters !@#$%^&*()`",
            HtmlOutput: "<p><code>inline code with special characters !@#$%^&amp;*()</code></p>"
        );
    }
}
