// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.ServerClient.Shared.ComponentLibrary;
using Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;

namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownParserTests {
    // see https://spec-md.com/
    [Test]
    [MethodDataSource(typeof(BlockQuoteDataSources), nameof(BlockQuoteDataSources.DataSources))]
    [MethodDataSource(typeof(CodeDataSources), nameof(CodeDataSources.DataSources))]
    [MethodDataSource(typeof(CodeInlineDataSources), nameof(CodeInlineDataSources.DataSources))]
    [MethodDataSource(typeof(EmphasisDataSources), nameof(EmphasisDataSources.DataSources))]
    [MethodDataSource(typeof(EscapedCharacterDataSources), nameof(EscapedCharacterDataSources.DataSources))]
    [MethodDataSource(typeof(HeadingDataSources), nameof(HeadingDataSources.DataSources))]
    [MethodDataSource(typeof(HorizontalLineDataSources), nameof(HorizontalLineDataSources.DataSources))]
    [MethodDataSource(typeof(HtmlDataSources), nameof(HtmlDataSources.DataSources))]
    [MethodDataSource(typeof(LinkDataSources), nameof(LinkDataSources.DataSources))]
    [MethodDataSource(typeof(ListsDataSources), nameof(ListsDataSources.DataSources))]
    [MethodDataSource(typeof(RandomDataSources), nameof(RandomDataSources.DataSources))]
    [MethodDataSource(typeof(SpecialCharacterDataSources), nameof(SpecialCharacterDataSources.DataSources))]
    [MethodDataSource(typeof(TableDataSources), nameof(TableDataSources.DataSources))]
    public async Task Parse_ValidInputs(MarkdownTestDto dto) {
        // Arrange
        var parser = new MarkdownParser();

        // Act
        string output = parser.Parse(dto.Markdown);

        // Assert
        await Assert.That(output).IsEqualTo(dto.HtmlOutput).IgnoringWhitespace();
    }
}
