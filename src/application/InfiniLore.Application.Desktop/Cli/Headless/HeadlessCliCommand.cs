// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Ansi;
using CodeOfChaos.CliArgsParser;

namespace InfiniLore.Application.Desktop.Cli.Headless;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CliData("headless")]
public partial class HeadlessCliCommand(ILogger<HeadlessCliCommand> logger, ICliParser parser) : ICliCommand<HeadlessCliParameters> {

    public async ValueTask ExecuteAsync(HeadlessCliParameters parameters, CancellationToken ct = new()) {
        if (!parameters.Console) {
            await Program.Application.WebApp.RunAsync();
            return;
        }
        
        var thread = new Thread(Program.Application.WebApp.Run);
        thread.Start();
        
        var builder = new AnsiStringBuilder();
        builder.Fore.AppendCyan("> ");
        string cursor = builder.ToStringAndClear();

        builder.Fore.AppendRedLine("Unknown command.");
        string unknownCommand = builder.ToStringAndClear();

        while (true) {
            Console.Write(cursor);
            if (Console.ReadLine() is not {} input) {
                Console.WriteLine(unknownCommand);
                continue;
            }

            try {
                await parser.ExecuteAsync(input, ct);
            }
            catch (Exception ex) {
                logger.Warning(ex, "Failed to execute command:  {command}", input);
            }
        }
    }
}
