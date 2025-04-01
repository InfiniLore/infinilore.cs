// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using InfiniLore.ServerClient.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Benchmarks.InfiniLore.ServerClient.Shared.ComponentLibrary;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Declared)]
public class IndividualMarkdownBenchmarks {
    private IMarkdownParser Parser { get; set; } = null!;

    [GlobalSetup]
    public void Setup() {
        var serviceCollection = new ServiceCollection();
        serviceCollection.RegisterServicesFromInfiniLoreServerClientShared();
        serviceCollection.AddLogging();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        Parser = serviceProvider.GetRequiredService<IMarkdownParser>();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Benchmarks for SinglelineStructuresRegex Features
    // -----------------------------------------------------------------------------------------------------------------

    [Benchmark]
    public string EscapedCharacters() {
        return Parser.Parse(@"\*escaped text\*");
    }

    [Benchmark]
    public string BoldAndItalic() {
        return Parser.Parse("***bold and italic***");
    }
    
    [Benchmark]
    public string BoldOnly() {
        return Parser.Parse("**bold**");
    }
    
    [Benchmark]
    public string ItalicOnly() {
        return Parser.Parse("*italic*");
    }
    
    [Benchmark]
    public string Superscript() {
        return Parser.Parse("^^sup-script^^");
    }
    
    [Benchmark]
    public string Subscript() {
        return Parser.Parse("^^sub-script^^");
    }
    
    [Benchmark]
    public string Strikethrough() {
        return Parser.Parse("~~strikethrough~~");
    }
    
    [Benchmark]
    public string Underline() {
        return Parser.Parse("_underline_");
    }
    
    [Benchmark]
    public string InlineCode() {
        return Parser.Parse("`inline code`");
    }
    
    [Benchmark]
    public string Emotes() {
        return Parser.Parse(":flag-trans:");
    }
    
    [Benchmark]
    public string NestedLinks() {
        return Parser.Parse("[![nested link](image_url)](outer_url)");
    }
    
    [Benchmark]
    public string RegularLinks() {
        return Parser.Parse("[Regular Link](https://example.com)");
    }
    
    [Benchmark]
    public string Tags() {
        return Parser.Parse("#tag");
    }
    
    [Benchmark]
    public string HtmlSpecialCharacters() {
        return Parser.Parse("&copy; & < >");
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Benchmarks for MultilineStructuresRegex Features
    // -----------------------------------------------------------------------------------------------------------------
    
    [Benchmark]
    public string Headings() {
        return Parser.Parse("""
            # Header 1
            ## Header 2
            ### Header 3
            """);
    }
    
    [Benchmark]
    public string CodeBlocks() {
        return Parser.Parse("""
            ```csharp
            code block
            ```
            """);
    }
    
    [Benchmark]
    public string SimpleHeadings() {
        return Parser.Parse("""
            Simple Heading
            ---
            """);
    }
    
    [Benchmark]
    public string UnorderedLists() {
        return Parser.Parse("""
            - Item 1
            - Item 2
              - Nested Item
            """);
    }
    
    [Benchmark]
    public string OrderedLists() {
        return Parser.Parse("""
            1. First Item
            2. Second Item
              3. Nested Item
            """);
    }
    
    [Benchmark]
    public string Tables() {
        return Parser.Parse(
            """
            | Column 1 | Column 2 |
            |----------|----------|
            | Value 1  | Value 2  |
            """
        );
    }
    
    [Benchmark]
    public string BlockQuotes() {
        return Parser.Parse("""
            > A blockquote
            > with multiple lines.
            """);
    }
    
    [Benchmark]
    public string HtmlBlocks() {
        return Parser.Parse("<div><p>HTML content</p></div>");
    }
    
    [Benchmark]
    public string HorizontalRules() {
        return Parser.Parse("""
            ---
            
            ***
            
            ___
            """);
    }
    
    [Benchmark]
    public string RemainderText() {
        return Parser.Parse("This is normal text left over.");
    }
}