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
        | (?<code>(?<!``)`([^`\n]+?)`(?!``))
        | (?<link>\[(.+?)\]\((.+?)\))
        | (?<heading>^(\#{1,6})\s(.+))
        | (?<codeBlock>```(.+?)\n([\s\S]*?)```(?!.*```))
        """, RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex MarkdownRegex { get; }

    public string Parse(string input) {
        string output = MarkdownRegex.Replace(input, static match => Evaluator(match));
        logger.Information("markdown input: {input} html output: {output}", input, output);
        return output;
    }

    private static string Evaluator(Match match, Origin origin = Origin.Undefined) {

        if (origin is not Origin.BoldAndItalic && match.Groups["boldAndItalic"].Success) {
            if (match.Groups[1] is { Success: true, Value: var chars1 }) {
                string output = MarkdownRegex.Replace(chars1, static m => Evaluator(m, Origin.BoldAndItalic));
                return $"<b><i>{output}</i></b>";
            }

            if (match.Groups[2] is { Success: true, Value: var chars2 }) {
                string output = MarkdownRegex.Replace(chars2, static m => Evaluator(m, Origin.BoldAndItalic));
                return $"<b>{output}</b>";
            }
        }

        if (origin is not Origin.Bold && match.Groups["bold"].Success) {
            if (match.Groups[3] is { Success: true, Value: var chars3 }) {
                string output = MarkdownRegex.Replace(chars3, static m => Evaluator(m, Origin.Bold));
                return $"<b>{output}</b>";
            }

            if (match.Groups[4] is { Success: true, Value: var chars4 }) {
                string output = MarkdownRegex.Replace(chars4, static m => Evaluator(m, Origin.Bold));
                return $"<b>{output}</b>";
            }
        }

        if (origin is not Origin.Italic && match.Groups["italic"].Success) {
            if (match.Groups[5] is { Success: true, Value: var chars5 }) {
                string output = MarkdownRegex.Replace(chars5, static m => Evaluator(m, Origin.Italic));
                return $"<i>{output}</i>";
            }

            if (match.Groups[6] is { Success: true, Value: var chars6 }) {
                string output = MarkdownRegex.Replace(chars6, static m => Evaluator(m, Origin.Italic));
                return $"<i>{output}</i>";
            }
        }

        if (origin is not Origin.Strike && match.Groups["strike"].Success && match.Groups[7] is { Success: true, Value: var chars7 }) {
            string output = MarkdownRegex.Replace(chars7, static m => Evaluator(m, Origin.Strike));
            return $"<s>{output}</s>";

        }

        if (match.Groups["code"].Success && match.Groups[8] is { Success: true, Value: var chars8 })
            return $"<code><pre>{chars8}</pre></code>"; // todo escape < and > characters

        if (match.Groups["link"].Success
            && match.Groups[9] is { Success: true, Value: var chars9 }
            && match.Groups[10] is { Success: true, Value: var chars10 }
        ) {
            return $"<a href=\"{chars9}\" target=\"_blank\">{chars10}</a>";
        }

        if (origin is not Origin.Heading && match.Groups["heading"].Success && match.Groups[11] is { Success: true, Length: var headingLevel } && match.Groups[12] is { Success: true, Value: var string12 }) {
            string output = MarkdownRegex.Replace(string12, m => Evaluator(m, Origin.Heading));
            return $"<h{headingLevel}>{output}</h{headingLevel}>";
        }

        if (origin is not Origin.CodeBlock && match.Groups["codeBlock"].Success && match.Groups[13] is { Success: true, Value: var string13 } && match.Groups[14] is { Success: true, ValueSpan: var chars14 }) {
            return $"<code><pre>{chars14}</pre></code>"; // todo escape < and > characters
        }

        return match.Value;
    }

    // # ***boldAndItalic*** **bold** *italic* ~~strike~~ `code` [text](https://example.com)  ___boldAndItalic___ __bold__ _italic_
    // ***boldAndItalic*** **bold** *italic* ~~strike~~ `code` [text](https://example.com)  ___boldAndItalic___ __bold__ _italic_
    // ** [text](https://example.com) **
    // # heading
    // ## headin
    // ### headi
    // #### head
    // ##### hea
    // ###### he
    
    private enum Origin {
        Undefined = 0,
        BoldAndItalic,
        Bold,
        Italic,
        Strike,
        Heading,
        CodeBlock
    }
}
