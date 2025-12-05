// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using Serilog;

namespace InfiniLore.Application;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniLoreCli(WebApplication webApp) {
    public void Run(string[] args) {
        ICliParserBuilder cliParserBuilder = webApp.GetInfiniLoreCliParserBuilder();
        cliParserBuilder.AddFromAssembly(typeof(Program).Assembly);
        ICliParser cliParser = cliParserBuilder.Build();
        try {
            Task.Run(async () => await cliParser.ExecuteAsync(args)).Wait();
        }
        catch (Exception e) {
            Log.Error(e, "Failed to parse command line arguments.");
            Environment.Exit(-1);
        }
    }
}
