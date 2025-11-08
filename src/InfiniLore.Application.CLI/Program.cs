// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
namespace InfiniLore.Application.CLI;
using CodeOfChaos.CliArgsParser;
using InfiniLore.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static async Task Main(string[] args) {
        var services = new ServiceCollection();
            
        services.AddInfiniLoreCore();
        
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        
        ICliParser parser = CliParser.CreateBuilder()
            .WithServiceProvider(serviceProvider)
            .AddFromAssembly<InfiniLoreCoreAssemblyEntry>()
            .Build();

        await parser.ExecuteAsync(args);
    }
}
