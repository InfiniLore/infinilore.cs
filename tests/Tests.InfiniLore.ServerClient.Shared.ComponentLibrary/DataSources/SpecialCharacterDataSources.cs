// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class SpecialCharacterDataSources {
    private static readonly string SectionName = nameof(SpecialCharacterDataSources)[..^nameof(DataSources).Length];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<MarkdownTestDto>> DataSources() {

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "",
            HtmlOutput: ""
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "&",
            HtmlOutput: "<p>&amp;</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "<",
            HtmlOutput: "<p>&lt;</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: ">",
            HtmlOutput: "<p>&gt;</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "&copy;",
            HtmlOutput: "<p>\u00a9</p>"
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "This contains an emoji: 😀",
            HtmlOutput: "<p>This contains an emoji: 😀</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "@username mentions",
            HtmlOutput: "<p>@username mentions</p>"
        );
    }
}
