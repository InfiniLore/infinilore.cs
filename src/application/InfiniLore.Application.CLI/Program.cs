// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Ansi;
using CodeOfChaos.CliArgsParser;
using FastEndpoints;
using InfiniLore.Core;
using InfiniLore.Modules.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger=Serilog.ILogger;

namespace InfiniLore.Application.CLI;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static async Task Main(string[] args) {
        var services = new ServiceCollection();

        services.AddInfiniLoreCore();
        services.AddLogging(config => {
            config.ClearProviders();
            config.AddSerilog();
        });
        services.AddSerilog(config => {
            config.WriteTo.Console();
        });
        services.AddFastEndpoints();

        ServiceProvider serviceProvider = services.BuildServiceProvider();

        ICliParser parser = CliParser.CreateBuilder()
            .WithServiceProvider(serviceProvider)
            .AddFromAssembly<InfiniLoreCoreAssemblyEntry>()
            .AddFromAssembly<InfiniLoreCoreModulesUsersAssemblyEntry>()
            .AddFromAssembly(typeof(Program).Assembly)
            .Build();

        if (args.Length != 0) {
            await parser.ExecuteAsync(args);
            return;
        }
        
        var builder = new AnsiStringBuilder();
        builder.Fore.AppendCyan("> ");
        string cursor = builder.ToStringAndClear();

        builder.Fore.AppendRedLine("Unknown command.");
        string unknownCommand = builder.ToStringAndClear();
        
        var logger = serviceProvider.GetRequiredService<ILogger>();
        
        while (true) {
            Console.Write(cursor);
            if (Console.ReadLine() is not {} input) {
                Console.WriteLine(unknownCommand);
                continue;
            }

            try {
                await parser.ExecuteAsync(input);
            }
            catch (Exception ex) {
                logger.Warning(ex, "Failed to execute command:  {command}", input);
            }
        }
    }
}
