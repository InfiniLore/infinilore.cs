// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using InfiniLore.ServerClient.Shared;
using Microsoft.Extensions.DependencyInjection;
using Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;

namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownParserTests {
    private static IMarkdownParser GetParser() {
        IServiceCollection services = new ServiceCollection()
            .RegisterServicesFromInfiniLoreServerClientShared();
        
        ServiceProvider provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IMarkdownParser>();
    }
    
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
        IMarkdownParser parser = GetParser();
        
        // Act
        string output = parser.Parse(dto.Markdown);

        // Assert
        await Assert.That(output).IsEqualTo(dto.HtmlOutput).IgnoringWhitespace();
    }
    
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
    public async Task Parse_ValidInputs_WithTextWriter(MarkdownTestDto dto) {
        // Arrange
        IMarkdownParser parser = GetParser();
        
        // Act
        await using var writer = new StringWriter();
        parser.Parse(dto.Markdown, writer);            
        string output = writer.ToString();             

        // Assert
        await Assert.That(output).IsEqualTo(dto.HtmlOutput).IgnoringWhitespace();
    }

}
