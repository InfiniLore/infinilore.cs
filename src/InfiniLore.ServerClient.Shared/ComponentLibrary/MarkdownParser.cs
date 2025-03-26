// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Buffers;
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
            ^\|(.+)\|\s*\r?\n
            ^\|([:\-|\ ]+)\|\s*\r?\n
            ((?:^\|.+\|\s*)+)
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
        if (match.Groups["remainder"].TryGetValue(out string? paragraph)) {
            string output = SinglelineStructuresRegex.Replace(paragraph, static m => SinglelineStructuresEvaluator(m));
            return $"<p>{output}</p>";
        }
        
        if (match.Groups["heading"].Success && match.Groups[1].TryGetLength(out int headingLevel) && match.Groups[2].TryGetValue(out string? headerText)) {
            string output = SinglelineStructuresRegex.Replace(headerText, static m => SinglelineStructuresEvaluator(m));
            return $"<h{headingLevel}>{output}</h{headingLevel}>";
        }

        if (match.Groups["codeBlock"].Success && match.Groups[3].TryGetValue(out string? langName) && match.Groups[4].TryGetValue(out string? codeBlockBody)) {
            string output = HtmlEncoder.Default.Encode(codeBlockBody);
            return $"<pre><code lang=\"{langName}\">{output}</code></pre>";
        }

        if (match.Groups["headingSimple"].Success && match.Groups[5].TryGetValue(out string? headerSimpleText)) {
            string output = SinglelineStructuresRegex.Replace(headerSimpleText, static m => SinglelineStructuresEvaluator(m));
            return $"<h1>{output}</h1>";
        }

        if (match.Groups["listUnordered"].TryGetValue(out string? listUnorderedBody)) {
            var builder = new StringBuilder(); // todo get and move to pool
            builder.Append("<ul>");
            foreach (Match lineMatch in ListItemBodyRegex.Matches(listUnorderedBody)) {
                builder.Append("<li>");
                if (lineMatch.Groups[1].TryGetValue(out string? listHeader)) {
                    string listHeaderOutput = SinglelineStructuresRegex.Replace(listHeader, static m => SinglelineStructuresEvaluator(m));
                    builder.Append(listHeaderOutput);
                }
                if (lineMatch.Groups[2].TryGetValue(out string? listBody)) {
                    string listBodyOutput = MultilineStructuresRegex.Replace(listBody, static m => MultilineStructuresEvaluator(m));
                    builder.Append(listBodyOutput);
                }
                
                builder.Append("</li>");
            }
            
            builder.Append("</ul>");
            return builder.ToString();
        }

        if (match.Groups["listOrdered"].TryGetValue(out string? listOrderedBody)) {
            var builder = new StringBuilder(); // todo get and move to pool
            builder.Append("<ol>");
            foreach (Match lineMatch in ListItemBodyRegex.Matches(listOrderedBody)) {
                builder.Append("<li>");
                
                if (lineMatch.Groups[1].TryGetValue(out string? listHeader)) {
                    string listHeaderOutput = SinglelineStructuresRegex.Replace(listHeader, static m => SinglelineStructuresEvaluator(m));
                    builder.Append(listHeaderOutput);
                }
                if (lineMatch.Groups[2].TryGetValue(out string? listBody)) {
                    string listBodyOutput = MultilineStructuresRegex.Replace(listBody, static m => MultilineStructuresEvaluator(m));
                    builder.Append(listBodyOutput);
                }
                
                builder.Append("</li>");
            }
            
            builder.Append("</ol>");
            return builder.ToString();
        }

        if (match.Groups["table"].Success) {
            // Extract header, separator, and rows
            ReadOnlySpan<char> header = match.Groups[6].ValueSpan;
            Span<Range> headerColumns = stackalloc Range[header.Length];
            int headerColumnCount = header.Split(headerColumns, '|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            ReadOnlySpan<char> separator = match.Groups[7].ValueSpan;

            ReadOnlySpan<char> rows = match.Groups[8].ValueSpan;
            Span<Range> rowRanges = stackalloc Range[rows.Length];
            int rowCount = rows.Split(rowRanges, '\n', StringSplitOptions.RemoveEmptyEntries);

            // Construct table HTML
            var builder = new StringBuilder();
            builder.Append("<table>");

            // Add headers
            builder.Append("<thead><tr>");
            for (int index = 0; index < headerColumnCount; index++) {
                ReadOnlySpan<char> column = header[headerColumns[index]];
                string output = SinglelineStructuresRegex.Replace(column.ToString(), static m => SinglelineStructuresEvaluator(m));
                builder.Append($"<th>{output}</th>");
            }
            builder.Append("</tr></thead>");

            // Add rows
            builder.Append("<tbody>");
            var bufferPool = ArrayPool<Range>.Shared;
            const int maxExpectedRowLength = 512; // Based on expected data characteristics
            Range[] rowColumnRanges = bufferPool.Rent(maxExpectedRowLength);

            try {
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++) {
                    Range rowRange = rowRanges[rowIndex];
                    ReadOnlySpan<char> row = rows[rowRange];

                    // Split the row
                    int rowColumnCount = row.Split(rowColumnRanges.AsSpan(0, row.Length), '|', 
                        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    builder.Append("<tr>");
                    for (int columnIndex = 0; columnIndex < rowColumnCount; columnIndex++) {
                        Range columnRange = rowColumnRanges[columnIndex];
                        ReadOnlySpan<char> column = row[columnRange];
                        string output = SinglelineStructuresRegex.Replace(column.ToString(), static m => SinglelineStructuresEvaluator(m));
                        builder.Append($"<td>{output}</td>");
                    }
                    builder.Append("</tr>");
                }
            }
            finally {
                bufferPool.Return(rowColumnRanges); // Ensure the buffer is returned, avoiding leaks
            }

            builder.Append("</tbody>");

            builder.Append("</table>");
            return builder.ToString();
        }

        return match.Value;
    }
    
    private static string SinglelineStructuresEvaluator(Match match, Origin origin = Origin.Undefined) {
        if (origin is not Origin.BoldAndItalic && match.Groups["boldAndItalic"].Success) {
            if (match.Groups[1].TryGetValue(out string? boldAndItalicValue)) {
                string output = SinglelineStructuresRegex.Replace(boldAndItalicValue, evaluator: static m => SinglelineStructuresEvaluator(m, Origin.BoldAndItalic));
                return $"<b><i>{output}</i></b>";
            }

            if (match.Groups[2].TryGetValue(out string? boldAndItalicUnderscoreValue)) {
                string output = SinglelineStructuresRegex.Replace(boldAndItalicUnderscoreValue, evaluator: static m => SinglelineStructuresEvaluator(m, Origin.BoldAndItalic));
                return $"<b>{output}</b>";
            }
        }

        if (origin is not Origin.Bold && match.Groups["bold"].Success) {
            if (match.Groups[3].TryGetValue(out string? boldValue)) {
                string output = SinglelineStructuresRegex.Replace(boldValue, evaluator: static m => SinglelineStructuresEvaluator(m, Origin.Bold));
                return $"<b>{output}</b>";
            }

            if (match.Groups[4].TryGetValue(out string? boldUnderscoreValue)) {
                string output = SinglelineStructuresRegex.Replace(boldUnderscoreValue, evaluator: static m => SinglelineStructuresEvaluator(m, Origin.Bold));
                return $"<b>{output}</b>";
            }
        }

        if (origin is not Origin.Italic && match.Groups["italic"].Success) {
            if (match.Groups[5].TryGetValue(out string? italicValue)) {
                string output = SinglelineStructuresRegex.Replace(italicValue, evaluator: static m => SinglelineStructuresEvaluator(m, Origin.Italic));
                return $"<i>{output}</i>";
            }

            if (match.Groups[6].TryGetValue(out string? italicUnderscoreValue)) {
                string output = SinglelineStructuresRegex.Replace(italicUnderscoreValue, evaluator: static m => SinglelineStructuresEvaluator(m, Origin.Italic));
                return $"<i>{output}</i>";
            }
        }

        if (origin is not Origin.Strike && match.Groups["strike"].Success && match.Groups[7].TryGetValue(out string? strikeValue)) {
            string output = SinglelineStructuresRegex.Replace(strikeValue, evaluator: static m => SinglelineStructuresEvaluator(m, Origin.Strike));
            return $"<s>{output}</s>";

        }

        if (match.Groups["code"].Success && match.Groups[8].TryGetValue(out string? codeValue)) {
            string output = HtmlEncoder.Default.Encode(codeValue);
            return $"<pre><code>{output}</code></pre>";
        }

        if (match.Groups["link"].Success
            && match.Groups[9].TryGetValue(out string? linkHref)
            && match.Groups[10].TryGetValue(out string? linkText)
        ) {
            return $"<a href=\"{linkHref}\" target=\"_blank\">{linkText}</a>";
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
