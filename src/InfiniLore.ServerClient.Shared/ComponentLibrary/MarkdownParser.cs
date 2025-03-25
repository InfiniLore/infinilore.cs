// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<MarkdownParser>(ServiceLifetime.Singleton)]
public partial class MarkdownParser(ILogger<MarkdownParser> logger) {
    [GeneratedRegex("""
          (?<bold>\*\*(.+?)\*\*)
        | (?<italic>\*(.+?)\*)
        | (?<italic_>_(.+?)_)
        | (?<strike>~~(.+?)~~)
        | (?<code>`(.+?)`)
        | (?<link>\[(.+?)\]\((.+?)\))
        | (?<heading>^(\#{1,6})\s(.+))
        """, RegexOptions.IgnorePatternWhitespace | RegexOptions.RightToLeft)]
    private static partial Regex MarkdownRegex { get; }

    public string Parse(string input) {
        string output = MarkdownRegex.Replace(input, Evaluator);
        
        logger.Information("markdown input: {input} html output: {output}", input, output);
        
        return output;
    }
    
    private string Evaluator(Match match) {
        if (match.Groups["bold"].Success) return $"<b>{match.Groups[1].Value}</b>";
        if (match.Groups["italic"].Success || match.Groups["italic_"].Success) return $"<i>{match.Groups[2].Value}</i>";
        if (match.Groups["strike"].Success) return $"<s>{match.Groups[4].Value}</s>";
        if (match.Groups["code"].Success) return $"<code>{match.Groups[5].Value}</code>";
        if (match.Groups["link"].Success) return $"<a href=\"{match.Groups[7].Value}\" target=\"_blank\">{match.Groups[6].Value}</a>";
        if (match.Groups["heading"].Success) {
            int headingLevel = match.Groups[8].Value.Length;
            string headingContent = Parse(match.Groups[9].Value);  // Recursively parse inner content (e.g., bold)
            return $"<h{headingLevel}>{headingContent}</h{headingLevel}>";
        }
        
        return match.Value;
    }
}
