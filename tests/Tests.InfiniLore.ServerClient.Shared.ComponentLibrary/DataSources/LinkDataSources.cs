// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class LinkDataSources {
    private static readonly string SectionName = nameof(LinkDataSources)[..^nameof(DataSources).Length];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<MarkdownTestDto>> DataSources() {
        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "This is an [-->*example*<--](https://www.facebook.com) of a link.",
            HtmlOutput: """<p>This is an <a href="https://www.facebook.com">--&gt;<em>example</em>&lt;--</a> of a link.</p>"""
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "![Specs](https://i.imgur.com/aV8o3rE.png)",
            HtmlOutput: "<p><img src=\"https://i.imgur.com/aV8o3rE.png\" alt=\"Specs\"></p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: "[![Specs](https://i.imgur.com/aV8o3rE.png)](https://imgur.com/)",
            HtmlOutput: """
            <p>
                <a href="https://imgur.com/">
                    <img src="https://i.imgur.com/aV8o3rE.png" alt="Specs">
                </a>
            </p>
            """
        );
    }
}
