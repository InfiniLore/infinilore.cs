// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class TagDataSources {
    private static readonly string SectionName = nameof(TagDataSources)[..^nameof(DataSources).Length];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<MarkdownTestDto>> DataSources() {
        yield return static () => new MarkdownTestDto(SectionName,
            "#tag",
            "<p><span>#tag</span></p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "**#tag**",
            "<p><strong><span>#tag</span></strong></p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "*#tag*",
            "<p><em><span>#tag</span></em></p>"
        );
    }
}
