// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.ServerClient.Shared.ComponentLibrary;
using Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.MultilineDataSources;

namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownParserTests {
    // see https://spec-md.com/
    [Test]
    [MethodDataSource(typeof(CodeDataSources), nameof(CodeDataSources.Data))]
    [MethodDataSource(typeof(HeadingDataSources), nameof(HeadingDataSources.Data))]
    [MethodDataSource(typeof(HorizontalLineDataSources), nameof(HorizontalLineDataSources.Data))]
    [MethodDataSource(typeof(HtmlDataSources), nameof(HtmlDataSources.Data))]
    [MethodDataSource(typeof(InlineDataSources), nameof(InlineDataSources.Data))]
    [MethodDataSource(typeof(ListsDataSources), nameof(ListsDataSources.Data))]
    [MethodDataSource(typeof(TableDataSources), nameof(TableDataSources.Data))]
    public async Task Parse_ValidInputs(MultilineDataDto dto) {
        // Arrange
        var parser = new MarkdownParser();

        // Act
        string output = parser.Parse(dto.Markdown);

        // Assert
        await Assert.That(output).IsEqualTo(dto.HtmlOutput).IgnoringWhitespace();
    }
}
