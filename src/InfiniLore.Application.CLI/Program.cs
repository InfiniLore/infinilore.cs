// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using InfiniLore.Core;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Application.CLI;
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
            .AddFromAssembly(typeof(Program).Assembly)
            .Build();

        await parser.ExecuteAsync(args);
    }
}
