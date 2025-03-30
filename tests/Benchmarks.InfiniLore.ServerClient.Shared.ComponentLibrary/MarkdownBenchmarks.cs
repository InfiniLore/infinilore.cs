// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using BenchmarkDotNet.Attributes;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using InfiniLore.ServerClient.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Benchmarks.InfiniLore.ServerClient.Shared.ComponentLibrary;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[MemoryDiagnoser]
public class MarkdownBenchmarks {
    private string Markdown { get; set; } = string.Empty;
    private IMarkdownParser Parser { get; set; } = null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    [GlobalSetup]
    public async Task Setup() {
        const string url = "https://gist.githubusercontent.com/allysonsilva/85fff14a22bbdf55485be947566cc09e/raw/fa8048a906ebed3c445d08b20c9173afd1b4a1e5/Full-Markdown.md";
        var client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);
        
        // Check that the first line has "# Headers"
        Markdown = await response.Content.ReadAsStringAsync();
        if (Markdown.IsNullOrWhiteSpace()) throw new InvalidOperationException("The Markdown input should not be empty.");

        string firstLine = Markdown.Split('\n')[0];
        if (!firstLine.StartsWith("# Headers")) throw new InvalidOperationException("The first line should start with '# Headers'.");
        
        var serviceCollection = new ServiceCollection();
        serviceCollection.RegisterServicesFromInfiniLoreServerClientShared();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        Parser = serviceProvider.GetRequiredService<IMarkdownParser>();
    }
    
    
    // [Benchmark(OperationsPerInvoke = 1000)]
    [Benchmark(Baseline = true)]
    public string RenderMarkdown() {
        string input = Markdown;
        
        string output = Parser.Parse(input);
        return output; 
    }
    
    [Benchmark]
    public StringWriter RenderMarkdownToStream() {
        var streamWriter = new StringWriter();

        Parser.Parse(Markdown, streamWriter);

        streamWriter.Flush();
        return streamWriter;

    }

}
