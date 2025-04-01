// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class StrikethroughDataSources {
    private static readonly string SectionName = nameof(StrikethroughDataSources)[..^nameof(DataSources).Length];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<MarkdownTestDto>> DataSources() {
        yield return static () => new MarkdownTestDto(
            SectionName,
            "~something~",
            "<p><s>something</s></p>"
        );

        yield return static () => new MarkdownTestDto(
            SectionName,
            @"\~escaped~",
            "<p>~escaped~</p>"
        );

        yield return static () => new MarkdownTestDto(
            SectionName,
            @"~escaped\~",
            "<p>~escaped~</p>"
        );

        yield return static () => new MarkdownTestDto(
            SectionName,
            "~nested ~strikethrough~ does not works~",
            "<p><s>nested</s>strikethrough<s> does not works</s></p>"
        );
        
        yield return static () => new MarkdownTestDto(
            SectionName,
            "~**bold and strike-through**~",
            "<p><s><strong>bold and strike-through</strong></s></p>"
        );
        
        yield return static () => new MarkdownTestDto(
            SectionName,
            "~*italic and strike-through*~",
            "<p><s><em>italic and strike-through</em></s></p>"
        );

        yield return static () => new MarkdownTestDto(
            SectionName,
            "**~bold strike~** normal ~strikethrough~",
            "<p><strong><s>bold strike</s></strong> normal <s>strikethrough</s></p>"
        );
        
        yield return static () => new MarkdownTestDto(
            SectionName,
            "~~",
            "<p>~~</p>"
        );
        
        yield return static () => new MarkdownTestDto(
            SectionName,
            "~one~ and ~two~!",
            "<p><s>one</s> and <s>two</s>!</p>"
        );

    }
}
