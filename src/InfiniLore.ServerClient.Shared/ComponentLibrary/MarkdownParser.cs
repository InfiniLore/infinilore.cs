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
        """, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline)]
    private static partial Regex MarkupRegex { get; }

    [GeneratedRegex("""
        (?<heading>^(\#{1,6})\s(.+))
        | (?<codeBlock>```(.+?)\n([\s\S]*?)```)
        | (?<headingSimple>^(.+?)\n[-]{3,})
        | (?<remainder>.+?(?:\n|$))
        """, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline)]
    private static partial Regex ParagraphRegex { get; }

    // private static void something() {
    //     builder.AddMarkdownEditor(config =>
    //         config.AddParser(@"(?<codeBlock>```(.+?)\n([\s\S]*?)```(?!.*```))", ExternalParsers.Codeblock)
    //         config.AddParser(@"(?<heading>^(\#{1,6})\s(.+))", ExternalParsers.Heading)
    //     );
    //     
    //     MarkdownParser.Regex = new Regex( string.join("|", config.Parsers.Select(p => p.Regex)), RegexOptions.Compiled);
    //
    //
    //     MarkdownParser.Parse(input, CombinedEvaluator);
    //     
    //     private void CombinedEvaluator(Match match) {
    //         foreach (var (groupname, handler) in MarkdownParser.Dictionary) {
    //             if (match.Groups[groupname].Success) handler.Handle(match);
    //         }
    //     }
    //
    //
    //
    // }

    public string Parse(string input) {
        string output = ParagraphRegex.Replace(input, evaluator: ParagraphEvaluator);
        
        logger.Information("markdown input: {input} html output: {output}", input, output);
        return output;
    }

    private static string MarkupRegexEvaluator(Match match, Origin origin = Origin.Undefined) {
        if (origin is not Origin.BoldAndItalic && match.Groups["boldAndItalic"].Success) {
            if (match.Groups[1] is { Success: true, Value: var chars1 }) {
                string output = MarkupRegex.Replace(chars1, evaluator: static m => MarkupRegexEvaluator(m, Origin.BoldAndItalic));
                return $"<b><i>{output}</i></b>";
            }

            if (match.Groups[2] is { Success: true, Value: var chars2 }) {
                string output = MarkupRegex.Replace(chars2, evaluator: static m => MarkupRegexEvaluator(m, Origin.BoldAndItalic));
                return $"<b>{output}</b>";
            }
        }

        if (origin is not Origin.Bold && match.Groups["bold"].Success) {
            if (match.Groups[3] is { Success: true, Value: var chars3 }) {
                string output = MarkupRegex.Replace(chars3, evaluator: static m => MarkupRegexEvaluator(m, Origin.Bold));
                return $"<b>{output}</b>";
            }

            if (match.Groups[4] is { Success: true, Value: var chars4 }) {
                string output = MarkupRegex.Replace(chars4, evaluator: static m => MarkupRegexEvaluator(m, Origin.Bold));
                return $"<b>{output}</b>";
            }
        }

        if (origin is not Origin.Italic && match.Groups["italic"].Success) {
            if (match.Groups[5] is { Success: true, Value: var chars5 }) {
                string output = MarkupRegex.Replace(chars5, evaluator: static m => MarkupRegexEvaluator(m, Origin.Italic));
                return $"<i>{output}</i>";
            }

            if (match.Groups[6] is { Success: true, Value: var chars6 }) {
                string output = MarkupRegex.Replace(chars6, evaluator: static m => MarkupRegexEvaluator(m, Origin.Italic));
                return $"<i>{output}</i>";
            }
        }

        if (origin is not Origin.Strike && match.Groups["strike"].Success && match.Groups[7] is { Success: true, Value: var chars7 }) {
            string output = MarkupRegex.Replace(chars7, evaluator: static m => MarkupRegexEvaluator(m, Origin.Strike));
            return $"<s>{output}</s>";

        }

        if (match.Groups["code"].Success && match.Groups[8] is { Success: true, Value: var chars8 })
            return $"<code><pre>{chars8}</pre></code>";// todo escape < and > characters

        if (match.Groups["link"].Success
            && match.Groups[9] is { Success: true, Value: var chars9 }
            && match.Groups[10] is { Success: true, Value: var chars10 }
        ) {
            return $"<a href=\"{chars9}\" target=\"_blank\">{chars10}</a>";
        }


        return match.Value;
    }

    private static string ParagraphEvaluator(Match match) {
        if (match.Groups["remainder"] is { Success: true, Value: var paragraph }) {
            string output = MarkupRegex.Replace(paragraph, static m => MarkupRegexEvaluator(m));
            return $"<p>{output}</p>";
        }
        
        if (match.Groups["heading"].Success && match.Groups[1] is { Success: true, Length: var headingLevel } && match.Groups[2] is { Success: true, Value: var string12 }) {
            string output = MarkupRegex.Replace(string12, static m => MarkupRegexEvaluator(m));
            return $"<h{headingLevel}>{output}</h{headingLevel}>";
        }

        if (match.Groups["codeBlock"].Success && match.Groups[3] is { Success: true, Value: var string13 } && match.Groups[4] is { Success: true, ValueSpan: var chars14 }) {
            return $"<code><pre>{chars14}</pre></code>";// todo escape < and > characters
        }

        if (match.Groups["headingSimple"].Success && match.Groups[5] is { Success: true, Value: var string15 }) {
            string output = MarkupRegex.Replace(string15, static m => MarkupRegexEvaluator(m));
            return $"<h1>{output}</h1>";
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
    }
}
