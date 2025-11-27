// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Ansi;
using CodeOfChaos.CliArgsParser;
using CodeOfChaos.Extensions.DependencyInjection;

namespace InfiniLore.Application.Desktop.Cli.Headless;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<SimpleTerminal>]
public class SimpleTerminal(ILogger<SimpleTerminal> logger, ICliParser parser) {
    private readonly List<string> _commandHistory = [];
    
    public async Task RunAsync(CancellationToken ct) {
        PrintWelcomeBanner();
        
        while (!ct.IsCancellationRequested) {
            try {
                PrintPrompt();
                
                string? input = Console.ReadLine();
                
                if (input.IsNullOrWhiteSpace()) continue;
                
                _commandHistory.Add(input);
                
                try {
                    if (HandleLocalCommand(input)) continue;
                    
                    await parser.ExecuteAsync(input, ct);
                    
                    var successBuilder = new AnsiStringBuilder();
                    successBuilder.Fore.AppendGreenLine("✓ Command executed successfully");
                    Console.Write(successBuilder.ToStringAndClear());
                }
                catch (Exception ex) {
                    logger.LogWarning(ex, "Failed to execute command: {Command}", input);
                    
                    var errorBuilder = new AnsiStringBuilder();
                    errorBuilder.Fore.AppendRedLine($"✗ Error: {ex.Message}");
                    Console.Write(errorBuilder.ToStringAndClear());
                }
            }
            catch (Exception ex) {
                logger.LogError(ex, "Terminal error");
            }
        }
    }
    
    private static void PrintWelcomeBanner() {
        var builder = new AnsiStringBuilder();
        builder.AppendLine(string.Empty);
        builder.Fore.AppendCyanLine("╔═══════════════════════════════════════════╗");
        builder.Fore.AppendCyanLine("║       InfiniLore Headless Console         ║");
        builder.Fore.AppendCyanLine("╚═══════════════════════════════════════════╝");
        builder.AppendLine(string.Empty);
        builder.Fore.AppendGreenLine("Web server is running in the background.");
        builder.Fore.AppendGrayLine("Commands: 'help' | 'exit' | 'clear' | 'history'");
        builder.AppendLine(string.Empty);
        Console.Write(builder.ToStringAndClear());
    }
    
    private static void PrintPrompt() {
        var builder = new AnsiStringBuilder();
        builder.Fore.AppendCyan("> ");
        Console.Write(builder.ToStringAndClear());
    }
    
    private bool HandleLocalCommand(string command) {
        string cmd = command.Trim().ToLowerInvariant();
        
        switch (cmd) {
            case "exit":
            case "quit":
                var exitBuilder = new AnsiStringBuilder();
                exitBuilder.Fore.AppendYellowLine("Shutting down...");
                Console.Write(exitBuilder.ToStringAndClear());
                Environment.Exit(0);
                return true;
            
            case "clear":
            case "cls":
                Console.Clear();
                PrintWelcomeBanner();
                return true;
            
            case "history":
                PrintHistory();
                return true;
            
            default:
                return false;
        }
    }
    
    private void PrintHistory() {
        if (_commandHistory.Count == 0) {
            var builder = new AnsiStringBuilder();
            builder.Fore.AppendGrayLine("No command history.");
            Console.WriteLine(builder.ToStringAndClear());
            return;
        }
        
        var historyBuilder = new AnsiStringBuilder();
        historyBuilder.AppendLine(string.Empty);
        historyBuilder.Fore.AppendCyanLine("Command History:");
        
        for (int i = 0; i < _commandHistory.Count; i++) {
            historyBuilder.Fore.AppendGray($"  {i + 1,3}. ");
            historyBuilder.AppendLine(_commandHistory[i]);
        }
        
        historyBuilder.AppendLine(string.Empty);
        Console.Write(historyBuilder.ToStringAndClear());
    }
}
