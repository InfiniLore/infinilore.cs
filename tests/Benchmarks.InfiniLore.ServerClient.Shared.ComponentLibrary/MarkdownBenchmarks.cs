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
    public void Setup() {
        string url = "https://gist.githubusercontent.com/allysonsilva/85fff14a22bbdf55485be947566cc09e/raw/fa8048a906ebed3c445d08b20c9173afd1b4a1e5/Full-Markdown.md";
        var client = new HttpClient();
        HttpResponseMessage response = client.GetAsync(url).Result;
        Markdown = response.Content.ReadAsStringAsync().Result;
        if (Markdown.IsNullOrWhiteSpace()) throw new InvalidOperationException("The Markdown input should not be empty.");
        
        var serviceCollection = new ServiceCollection();
        serviceCollection.RegisterServicesFromInfiniLoreServerClientShared();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        Parser = serviceProvider.GetRequiredService<IMarkdownParser>();
    }
    
    
    // [Benchmark(OperationsPerInvoke = 1000)]
    [Benchmark()]
    public string RenderMarkdown() {
        string input = Markdown;
        
        string output = Parser.ParseMultiline(input);
        return output; 
    }
}
