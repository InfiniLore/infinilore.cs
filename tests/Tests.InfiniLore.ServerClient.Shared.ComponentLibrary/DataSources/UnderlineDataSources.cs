// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UnderlineDataSources {
    private static readonly string SectionName = nameof(UnderlineDataSources)[..^nameof(DataSources).Length];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<MarkdownTestDto>> DataSources() {
        yield return static () => new MarkdownTestDto(SectionName,
            "something _underlined_",
            """
            <p>something <span style="text-decoration: underline;">underlined</span></p>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            "something _underlined with an \\_ escaped_",
            """
            <p>something <span style="text-decoration: underline;">underlined with an _ escaped</span></p>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            "something _**bold and underlined**_",
            """
            <p>something <span style="text-decoration: underline;"><strong>bold and underlined</strong></span></p>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            "something _*italic and underlined*_",
            """
            <p>something <span style="text-decoration: underline;"><em>italic and underlined</em></span></p>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            "something _***bold, italic, and underlined***_",
            """
            <p>something <span style="text-decoration: underline;"><strong><em>bold, italic, and underlined</em></strong></span></p>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            "click _[here](https://example.com)_",
            """
            <p>click <span style="text-decoration: underline;"><a href="https://example.com">here</a></span></p>
            """
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "something _**[bold link](https://example.com)**_",
            """
            <p>something <span style="text-decoration: underline;"><strong><a href="https://example.com">bold link</a></strong></span></p>
            """
        );
        
        yield return static () => new MarkdownTestDto(
            SectionName,
            "this has _**multiple** elements to *underline*_",
            """
            <p>this has <span style="text-decoration: underline;"><strong>multiple</strong> elements to<em> underline</em></span></p>
            """
        );
        
        yield return static () => new MarkdownTestDto(
            SectionName,
            "__",
            "<p>__</p>"
        );
        
        yield return static () => new MarkdownTestDto(
            SectionName,
            @"\_escaped_",
            "<p>_escaped_</p>"
        );

        yield return static () => new MarkdownTestDto(
            SectionName,
            @"_escaped\_",
            "<p>_escaped_</p>"
        );


    }
}
