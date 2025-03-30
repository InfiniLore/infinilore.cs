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
            "",
            ""
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "&",
            "<p>&amp;</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "<",
            "<p>&lt;</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            ">",
            "<p>&gt;</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "&copy;",
            "<p>\u00a9</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "This contains an emoji: 😀",
            "<p>This contains an emoji: 😀</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "@username mentions",
            "<p>@username mentions</p>"
        );
    }
}
