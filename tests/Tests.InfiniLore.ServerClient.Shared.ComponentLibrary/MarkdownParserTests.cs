// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.ServerClient.Shared.ComponentLibrary;

namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownParserTests {
    
    // see https://spec-md.com/
    [Test]
    [Arguments("", "")]
    [Arguments("&", "<p>&amp;</p>")]
    [Arguments("<", "<p>&lt;</p>")]
    [Arguments(">", "<p>&gt;</p>")]
    [Arguments("&copy;", "<p>\u00a9</p>")]
    [Arguments(@"\*literal asterisks\*", "<p>*literal asterisks*</p>")]
    [Arguments(@"\!\""\#\$\%\&\'\(\)\*\+\,\-\.\/\:\;\<\=\>\?\@\[\\\]\^\_\`\{\|\}\~", "<p>!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~</p>")]
    [Arguments("This is an [-->*example*<--](https://www.facebook.com) of a link.", """<p>This is an <a href="https://www.facebook.com">--><em>example</em><--</a> of a link.</p>""")]
    [Arguments("Example of **bold** and *italic* and ***bold italic***.", "<p>Example of <strong>bold</strong> and <em>italic</em> and <strong><em>bold italic</em></strong>.</p>")]
    [Arguments("This is an `example` of some inline code.", "<p>This is an <code>example</code> of some inline code.</p>")]
    [Arguments("![Specs](http://i.imgur.com/aV8o3rE.png)", "<p><img src=\"http://i.imgur.com/aV8o3rE.png\" alt=\"Specs\"></p>")]
    public async Task SimpleMarkdown(string markdown, string htmlOutput) {
        // Arrange
        var parser = new MarkdownParser();

        // Act
        string output = parser.Parse(markdown);

        // Assert
        await Assert.That(output).IsEqualTo(htmlOutput);
    }
}
