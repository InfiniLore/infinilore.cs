// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class EscapedCharacterDataSources {
    private static readonly string SectionName = nameof(EscapedCharacterDataSources)[..^nameof(DataSources).Length];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<MarkdownTestDto>> DataSources() {

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: @"\*literal asterisks\*",
            HtmlOutput: "<p>*literal asterisks*</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: @"\!\""\#\$\%\&\'\(\)\*\+\,\-\.\/\:\;\<\=\>\?\@\[\\\]\^\_\`\{\|\}\~",
            HtmlOutput: "<p>!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "\\\"She told me that \\'he isn't here right *now*\\' - so I left.\\\"",
            HtmlOutput: "<p>\"She told me that 'he isn't here right <em>now</em>' - so I left.\"</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: @"Escape characters like backticks: \`code\`",
            HtmlOutput: "<p>Escape characters like backticks: `code`</p>"
        );
    }
}
