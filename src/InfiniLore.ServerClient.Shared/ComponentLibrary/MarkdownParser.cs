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
        (?<boldAndItalic>(?<!\*)\*\*\*([^*]+?)\*\*\*(?!\*)|(?<!_)___([^_]+?)___(?!_))
        | (?<bold>(?<!\*)\*\*([^*]+?)\*\*(?!\*)|(?<!_)__([^_]+?)__(?!_))
        | (?<italic>(?<!\*)\*([^*]+?)\*(?!\*)|(?<!_)_([^_]+?)_(?!_))
        | (?<strike>~~(.+?)~~)
        | (?<code>`(.+?)`)
        | (?<link>\[(.+?)\]\((.+?)\))
        | (?<heading>^(\#{1,6})\s(.+))
        """, RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex MarkdownRegex { get; }

    public string Parse(string input) {
        string output = MarkdownRegex.Replace(input, Evaluator);
        
        logger.Information("markdown input: {input} html output: {output}", input, output);
        
        return output;
    }
    
    private string Evaluator(Match match) {
        
        if (match.Groups["boldAndItalic"].Success) {
            if (match.Groups[1] is {Success: true, ValueSpan: var chars1})
                return $"<b><i>{chars1}</i></b>";
            if (match.Groups[2] is {Success: true, ValueSpan: var chars2})
                return $"<b><i>{chars2}</i></b>";
        }

        if (match.Groups["bold"].Success) {
            if (match.Groups[3] is {Success: true, ValueSpan: var chars3})
                return $"<b>{chars3}</b>";
            if (match.Groups[4] is {Success: true, ValueSpan: var chars4})
                return $"<b>{chars4}</b>";
        }

        if (match.Groups["italic"].Success) {
            if (match.Groups[5] is {Success: true, ValueSpan: var chars5})
                return $"<i>{chars5}</i>";
            if (match.Groups[6] is {Success: true, ValueSpan: var chars6})
                return $"<i>{chars6}</i>";
        }

        if (match.Groups["strike"].Success && match.Groups[7] is {Success: true, ValueSpan: var chars7})
            return $"<s>{chars7}</s>";

        if (match.Groups["code"].Success && match.Groups[8] is {Success: true, ValueSpan: var chars8})
            return $"<code>{chars8}</code>";

        if (match.Groups["link"].Success 
            && match.Groups[9] is {Success: true, ValueSpan: var chars9} 
            && match.Groups[10] is {Success: true, ValueSpan: var chars10}
        ) {
            return $"<a href=\"{chars9}\" target=\"_blank\">{chars10}</a>";
        }

        if (match.Groups["heading"].Success && match.Groups[11] is {Success: true, Length: var headingLevel} && match.Groups[12] is {Success: true, Value: var string12}) {
            string headingContent = Parse(string12); // Recursively parse inner content (e.g., bold)
            return $"<h{headingLevel}>{headingContent}</h{headingLevel}>";
        }
        
        return match.Value;
        
        // # ***boldAndItalic*** **bold** *italic* ~~strike~~ `code` [text](https://example.com)  ___boldAndItalic___ __bold__ _italic_
        // ***boldAndItalic*** **bold** *italic* ~~strike~~ `code` [text](https://example.com)  ___boldAndItalic___ __bold__ _italic_
        // # heading
        // ## headin
        // ### headi
        // #### head
        // ##### hea
        // ###### he
    }
}
