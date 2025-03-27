// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.MultilineDataSources;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class InlineDataSources {
    public static IEnumerable<Func<MultilineDataDto>> Data() {

        yield return static () => new MultilineDataDto(
            Markdown: "",
            HtmlOutput: ""
        );

        yield return static () => new MultilineDataDto(
            Markdown: "&",
            HtmlOutput: "<p>&amp;</p>"
        );

        yield return static () => new MultilineDataDto(
            Markdown: "<",
            HtmlOutput: "<p>&lt;</p>"
        );

        yield return static () => new MultilineDataDto(
            Markdown: ">",
            HtmlOutput: "<p>&gt;</p>"
        );

        yield return static () => new MultilineDataDto(
            Markdown: "&copy;",
            HtmlOutput: "<p>\u00a9</p>"
        );

        yield return static () => new MultilineDataDto(
            Markdown: @"\*literal asterisks\*",
            HtmlOutput: "<p>*literal asterisks*</p>"
        );

        yield return static () => new MultilineDataDto(
            Markdown: @"\!\""\#\$\%\&\'\(\)\*\+\,\-\.\/\:\;\<\=\>\?\@\[\\\]\^\_\`\{\|\}\~",
            HtmlOutput: "<p>!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~</p>"
        );

        yield return static () => new MultilineDataDto(
            Markdown: "This is an [-->*example*<--](https://www.facebook.com) of a link.",
            HtmlOutput: """<p>This is an <a href="https://www.facebook.com">--><em>example</em><--</a> of a link.</p>"""
        );

        yield return static () => new MultilineDataDto(
            Markdown: "Example of **bold** and *italic* and ***bold italic***.",
            HtmlOutput: "<p>Example of <strong>bold</strong> and <em>italic</em> and <strong><em>bold italic</em></strong>.</p>"
        );

        yield return static () => new MultilineDataDto(
            Markdown: "This is an `example` of some inline code.",
            HtmlOutput: "<p>This is an <code>example</code> of some inline code.</p>"
        );

        yield return static () => new MultilineDataDto(
            Markdown: "![Specs](https://i.imgur.com/aV8o3rE.png)",
            HtmlOutput: "<p><img src=\"https://i.imgur.com/aV8o3rE.png\" alt=\"Specs\"></p>"
        );
        
        yield return static () => new MultilineDataDto(
            Markdown: "This contains an emoji: 😀",
            HtmlOutput: "<p>This contains an emoji: 😀</p>"
        );
        
        yield return static () => new MultilineDataDto(
            Markdown: @"Escape characters like backticks: \`code\`",
            HtmlOutput: "<p>Escape characters like backticks: `code`</p>"
        );

        yield return static () => new MultilineDataDto(
            Markdown: "@username mentions",
            HtmlOutput: "<p>@username mentions</p>"
        );
    }
}
