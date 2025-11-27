// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;

namespace InfiniLore.Application.Desktop.Cli.Headless;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CliData("headless")]
public partial class HeadlessCliCommand(SimpleTerminal simpleTerminal) : ICliCommand<HeadlessCliParameters> {

    public async ValueTask ExecuteAsync(HeadlessCliParameters parameters, CancellationToken ct = new()) {
        if (!parameters.Console) {
            await Program.Application.WebApp.RunAsync();
            return;
        }
        
        Task appTask = Task.Run(() => Program.Application.WebApp.Run(), ct);
        await Task.Delay(2000, ct);
        
        await simpleTerminal.RunAsync(ct);
        
        await appTask;
    }
}