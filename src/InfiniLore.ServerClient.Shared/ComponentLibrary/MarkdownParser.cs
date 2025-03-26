// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<MarkdownParser>(ServiceLifetime.Singleton)]
public partial class MarkdownParser(ILogger<MarkdownParser> logger) {
    [GeneratedRegex("""
          (?<boldAndItalic>\*\*\*([^*]+?)\*\*\*|___([^_]+?)___)
        | (?<bold>\*\*([^*]+?)\*\*|__([^_]+?)__)
        | (?<italic>\*([^*]+?)\*|_([^_]+?)_)
        | (?<strike>~~(.+?)~~)
        | (?<code>`((?:[^`\\]|\\`)+?)`)
        | (?<link>\[(.+?)\]\((.+?)\))
        """, RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex SinglelineStructuresRegex { get; }

    [GeneratedRegex("""
          (?<heading>^(\#{1,6})\s(.+))
        | (?<codeBlock>```(.+?)\n([\s\S]*?)```)
        | (?<headingSimple>^(.+?)\n\s*[-=]{3,})
        | (?<listUnordered>(?:^[^\S\r\n]*[*+-]\s+.+(?:(?:\n[^\S\r\n]*[*+-.]\d*\s+.+)|(?:\n[^\S\r\n]+.+))*(?:[^\S\r\n]{0,2}(?![\r\n]))?)+)
        | (?<listOrdered>(?:^[^\S\r\n]*[*+-.]\d+\s+.+(?:(?:\n[^\S\r\n]*[*+-.]\d+\s+.+)|(?:\n[^\S\r\n]+.+))*(?:[^\S\r\n]{0,2}(?![\r\n]))?)+)
        | (?<table>
            (?:\|(?:\ *(\w)*\ *\|)+)\s
            (?:\|(?:\ *-+\ *\|)+)\s
            (?:\|(?:\ *(\w)*\ *\|)+)
          )
        | (?<remainder>.+?(?:\n|$))
        """, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline )]
    private static partial Regex MultilineStructuresRegex { get; }
    
    [GeneratedRegex(@"^[ ]*[*+-.]\d*\s+(.+)((?:(?:(?:\n[ ]+[*+-.]\d*)|(?:\n[ ]+))\s+.+)*)", RegexOptions.Multiline)]
    private static partial Regex ListItemBodyRegex { get; }

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
    // }

    public string Parse(string input) {
        string output = MultilineStructuresRegex.Replace(input, evaluator: MultilineStructuresEvaluator);
        
        logger.Information("markdown input: {input} html output: {output}", input, output);
        return output;
    }

    private static string MultilineStructuresEvaluator(Match match) {
        if (match.Groups["remainder"] is { Success: true, Value: var paragraph }) {
            string output = SinglelineStructuresRegex.Replace(paragraph, static m => SinglelineStructuresEvaluator(m));
            return $"<p>{output}</p>";
        }
        
        if (match.Groups["heading"].Success && match.Groups[1] is { Success: true, Length: var headingLevel } && match.Groups[2] is { Success: true, Value: var string12 }) {
            string output = SinglelineStructuresRegex.Replace(string12, static m => SinglelineStructuresEvaluator(m));
            return $"<h{headingLevel}>{output}</h{headingLevel}>";
        }

        if (match.Groups["codeBlock"].Success && match.Groups[3] is { Success: true, Value: var string13 } && match.Groups[4] is { Success: true, Value: var chars14 }) {
            string output = HtmlEncoder.Default.Encode(chars14);
            return $"<pre><code>{output}</code></pre>";
        }

        if (match.Groups["headingSimple"].Success && match.Groups[5] is { Success: true, Value: var string15 }) {
            string output = SinglelineStructuresRegex.Replace(string15, static m => SinglelineStructuresEvaluator(m));
            return $"<h1>{output}</h1>";
        }

        if (match.Groups["listUnordered"] is { Success: true, Value: var string11 }) {
            var builder = new StringBuilder(); // todo get and move to pool
            builder.Append("<ul>");
            foreach (Match lineMatch in ListItemBodyRegex.Matches(string11)) {
                builder.Append("<li>");
                string lineHeader = SinglelineStructuresRegex.Replace(lineMatch.Groups[1].Value, static m => SinglelineStructuresEvaluator(m));
                builder.Append(lineHeader);

                if (lineMatch.Groups[2] is { Success: true, Value: var string16 }) {
                    string listBody = MultilineStructuresRegex.Replace(string16, static m => MultilineStructuresEvaluator(m));
                    builder.Append(listBody);
                }
                
                builder.Append("</li>");
            }
            
            builder.Append("</ul>");
            return builder.ToString();
        }

        if (match.Groups["listOrdered"] is { Success: true, Value: var string17 }) {
            var builder = new StringBuilder(); // todo get and move to pool
            builder.Append("<ol>");
            foreach (Match lineMatch in ListItemBodyRegex.Matches(string17)) {
                builder.Append("<li>");
                string lineHeader = SinglelineStructuresRegex.Replace(lineMatch.Groups[1].Value, static m => SinglelineStructuresEvaluator(m));
                builder.Append(lineHeader);

                if (lineMatch.Groups[2] is { Success: true, Value: var string16 }) {
                    string listBody = MultilineStructuresRegex.Replace(string16, static m => MultilineStructuresEvaluator(m));
                    builder.Append(listBody);
                }
                
                builder.Append("</li>");
            }
            
            builder.Append("</ol>");
            return builder.ToString();
        }

        if (match.Groups["table"] is { Success: true, Value: var string18 } tableGroup) {
            var builder = new StringBuilder(); // todo get and move to pool
            builder.Append("<table>");

            foreach (Capture tableGroupCapture in tableGroup.Captures) {
                builder.Append("<tr>");
                builder.Append("<th>");
            }

        }

        return match.Value;
    }
    
    private static string SinglelineStructuresEvaluator(Match match, Origin origin = Origin.Undefined) {
        if (origin is not Origin.BoldAndItalic && match.Groups["boldAndItalic"].Success) {
            if (match.Groups[1] is { Success: true, Value: var chars1 }) {
                string output = SinglelineStructuresRegex.Replace(chars1, evaluator: static m => SinglelineStructuresEvaluator(m, Origin.BoldAndItalic));
                return $"<b><i>{output}</i></b>";
            }

            if (match.Groups[2] is { Success: true, Value: var chars2 }) {
                string output = SinglelineStructuresRegex.Replace(chars2, evaluator: static m => SinglelineStructuresEvaluator(m, Origin.BoldAndItalic));
                return $"<b>{output}</b>";
            }
        }

        if (origin is not Origin.Bold && match.Groups["bold"].Success) {
            if (match.Groups[3] is { Success: true, Value: var chars3 }) {
                string output = SinglelineStructuresRegex.Replace(chars3, evaluator: static m => SinglelineStructuresEvaluator(m, Origin.Bold));
                return $"<b>{output}</b>";
            }

            if (match.Groups[4] is { Success: true, Value: var chars4 }) {
                string output = SinglelineStructuresRegex.Replace(chars4, evaluator: static m => SinglelineStructuresEvaluator(m, Origin.Bold));
                return $"<b>{output}</b>";
            }
        }

        if (origin is not Origin.Italic && match.Groups["italic"].Success) {
            if (match.Groups[5] is { Success: true, Value: var chars5 }) {
                string output = SinglelineStructuresRegex.Replace(chars5, evaluator: static m => SinglelineStructuresEvaluator(m, Origin.Italic));
                return $"<i>{output}</i>";
            }

            if (match.Groups[6] is { Success: true, Value: var chars6 }) {
                string output = SinglelineStructuresRegex.Replace(chars6, evaluator: static m => SinglelineStructuresEvaluator(m, Origin.Italic));
                return $"<i>{output}</i>";
            }
        }

        if (origin is not Origin.Strike && match.Groups["strike"].Success && match.Groups[7] is { Success: true, Value: var chars7 }) {
            string output = SinglelineStructuresRegex.Replace(chars7, evaluator: static m => SinglelineStructuresEvaluator(m, Origin.Strike));
            return $"<s>{output}</s>";

        }

        if (match.Groups["code"].Success && match.Groups[8] is { Success: true, Value: var chars8 }) {
            string output = HtmlEncoder.Default.Encode(chars8);
            return $"<pre><code>{output}</code></pre>";
        }

        if (match.Groups["link"].Success
            && match.Groups[9] is { Success: true, Value: var chars9 }
            && match.Groups[10] is { Success: true, Value: var chars10 }
        ) {
            return $"<a href=\"{chars9}\" target=\"_blank\">{chars10}</a>";
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

    // TODO should be flags
    private enum Origin {
        Undefined = 0,
        BoldAndItalic,
        Bold,
        Italic,
        Strike,
    }
}
